<template>
  <Suspense>
    <LoadingScreen v-if="!isAuthReady" />
    <RouterView v-else />
  </Suspense>
</template>

<script setup lang="ts">
import { useAuth } from './composables/useAuth'
import LoadingScreen from '@/components/LoadingScreen.vue'

const { isAuthReady, initializeAuth } = useAuth()

initializeAuth()
</script>
