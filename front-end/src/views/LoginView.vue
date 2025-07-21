<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white shadow-md rounded px-8 py-6 w-full max-w-md">
      <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
      <form @submit.prevent="login">
        <input v-model="username" type="text" placeholder="Username"
          class="w-full mb-4 px-4 py-2 border rounded" required />
        <input v-model="password" type="password" placeholder="Password"
          class="w-full mb-4 px-4 py-2 border rounded" required />
        <button type="submit" class="w-full bg-black text-white py-2 rounded hover:bg-gray-800 transition">
          Login
        </button>
        <p class="mt-4 text-sm text-center">
          Don't have an account?
          <router-link to="/register" class="text-blue-600 hover:underline">Register</router-link>
        </p>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const username = ref('')
const password = ref('')
const router = useRouter()

const login = async () => {
  try {
    const res = await axios.post('http://localhost:5280/api/Auth/login', {
      username: username.value,
      password: password.value
    })
    localStorage.setItem('token', res.data.token)
    axios.defaults.headers.common['Authorization'] = `Bearer ${res.data.token}`
    router.push('/')
  } catch (err) {
    alert('Login failed. Please check your credentials.')
    console.error(err)
  }
}
</script>
