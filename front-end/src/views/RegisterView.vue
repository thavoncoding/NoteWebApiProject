<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100">
    <div class="bg-white shadow-md rounded px-8 py-6 w-full max-w-md">
      <h2 class="text-2xl font-bold mb-6 text-center">Register</h2>
      <form @submit.prevent="register">
        <input v-model="username" type="text" placeholder="Username"
          class="w-full mb-4 px-4 py-2 border rounded" required />
        <input v-model="password" type="password" placeholder="Password"
          class="w-full mb-4 px-4 py-2 border rounded" required />
        <button type="submit" class="w-full bg-black text-white py-2 rounded hover:bg-gray-800 transition">
          Register
        </button>
        <p class="mt-4 text-sm text-center">
          Already have an account?
          <router-link to="/login" class="text-blue-600 hover:underline">Login</router-link>
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

const register = async () => {
  try {
    await axios.post('http://localhost:5280/api/Auth/register', {
      username: username.value,
      password: password.value
    })
    alert('Registration successful! Please login.')
    router.push('/login')
  } catch (err) {
    alert('Registration failed. Try a different username.')
    console.error(err)
  }
}
</script>
