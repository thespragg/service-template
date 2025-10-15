<template>
  <div class="min-h-screen bg-slate-950 flex items-center justify-center">
    <div class="text-center">
      <div class="mb-8">
        <div class="w-16 h-16 border-4 border-slate-600 border-t-slate-400 rounded-full animate-spin mx-auto"></div>
      </div>
      <h2 class="text-2xl font-semibold text-slate-200 mb-2">
        Loading {{APP_NAME}}
      </h2>
      <p class="text-slate-400">
        Initializing{{ '.'.repeat(dots) }}
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';

const dots = ref(0);

onMounted(() => {
  const interval = setInterval(() => {
    dots.value = (dots.value + 1) % 4;
  }, 500);

  return () => clearInterval(interval);
});
</script>
