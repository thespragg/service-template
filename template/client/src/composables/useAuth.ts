import { ref, readonly } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';
import type { User } from '@/types/api';
import { use{{APP_NAME}}Api } from '@/api';

const isAuthReady = ref(false);
const authToken = ref<string | null>(null);
let initPromise: Promise<void> | null = null;
const user = ref<User>();

export function useAuth() {
  const auth0 = useAuth0();

  const initializeAuth = async () => {
    if (initPromise) return initPromise;

    initPromise = (async () => {
      try {
        await new Promise<void>((resolve) => {
          const checkLoading = () => {
            if (!auth0.isLoading.value) {
              resolve();
            } else {
              setTimeout(checkLoading, 500);
            }
          };
          checkLoading();
        });

        try {
          await auth0.checkSession();
        } catch { }
        
        if (auth0.isAuthenticated.value) {
          authToken.value = await auth0.getAccessTokenSilently();
          const api = use{{APP_NAME}}Api();
          user.value = await api.users.getCurrent()
        }

        isAuthReady.value = true;
      } catch (error) {
        console.error('Auth initialization error:', error);
      }
    })();

    return initPromise;
  };

  return {
    isAuthReady: readonly(isAuthReady),
    authToken: readonly(authToken),
    initializeAuth,
    ...auth0,
    user: user.value!
  };
}
