<template>
  <div>
    <pre class="text-white"> {{ user }}</pre>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from '@/composables/useAuth'

const { user } = useAuth()
</script>
