<template>
  <div>
    <pre class="text-white"> {{ user }}</pre>
  </div>
</template>

<script setup lang="ts">
import { use{{APP_NAME}}Api } from '@/api';

const { users } = use{{APP_NAME}}Api()
const user = await users.getCurrent();
console.log(user)
</script>
