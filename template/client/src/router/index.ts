import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import { useAuth } from '@/composables/useAuth'

const CallbackView = () => import('@/views/CallbackView.vue')

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: HomeView,
    },
    {
      path: '/callback',
      name: 'Callback',
      component: CallbackView,
      meta: { public: true }
    },
  ],
})

router.beforeEach(async (to, from, next) => {
  const { isAuthReady, isAuthenticated, loginWithRedirect } = useAuth()
  
  if (to.meta.public) {
    return true
  }
  
  if (!isAuthReady.value) {
    await new Promise<void>((resolve) => {
      const checkReady = setInterval(() => {
        if (isAuthReady.value) {
          clearInterval(checkReady)
          resolve()
        }
      }, 50)
    })
  }
  
  if (isAuthenticated.value) {
    return true
  } else {
    loginWithRedirect({
      appState: { targetUrl: to.fullPath }
    })
    return false
  }
})

export default router
