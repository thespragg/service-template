import { ref, readonly } from 'vue';
import { useAuth0 } from '@auth0/auth0-vue';

const isAuthReady = ref(false);
const authToken = ref<string | null>(null);
let initPromise: Promise<void> | null = null;

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

        if (auth0.isAuthenticated.value) {
          authToken.value = await auth0.getAccessTokenSilently();
          isAuthReady.value = true;
        }
      } catch (error) {
        console.error('Auth initialization error:', error);
      }
    })();

    return initPromise;
  };

  const refreshToken = async () => {
    if (auth0.isAuthenticated.value) {
      try {
        authToken.value = await auth0.getAccessTokenSilently({ cacheMode: 'off' });
      } catch (error) {
        console.error('Token refresh error:', error);
      }
    }
  };

  return {
    isAuthReady: readonly(isAuthReady),
    authToken: readonly(authToken),
    initializeAuth,
    refreshToken,
    ...auth0,
  };
}