<template>
  <div class="min-h-screen flex flex-col justify-center items-center bg-white">
    <div class="bg-white border border-gray-200 shadow-2xl rounded-3xl px-8 py-12 mt-8 w-full max-w-md animate-fade-in-up">
      <h2 class="text-3xl font-extrabold text-black mb-8 text-center tracking-tight">Welcome Back</h2>
      <p class="text-center text-gray-600 mb-8">Sign in to your account</p>

      <div class="space-y-6">
        <input
          v-model="username"
          type="text"
          placeholder="Username"
          class="w-full px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition"
        />
        <input
          v-model="password"
          type="password"
          placeholder="Password"
          class="w-full px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition"
        />
        <button
          @click="login"
          class="w-full py-3 rounded-xl bg-black text-white font-bold text-lg shadow hover:bg-gray-800 transition"
        >
          Sign In
        </button>
      </div>

      <p class="text-center text-red-500 mt-6 font-medium min-h-[1.5rem]" v-if="message">{{ message }}</p>
      <div class="mt-8 text-center">
          <p class="text-gray-600">Don't have an account yet?</p>
          <button
            @click="router.push('/register')"
            class="mt-2 text-black font-semibold hover:underline focus:outline-none transition"
          >
            Register
          </button>
        </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'

const router = useRouter()

const username = ref('')
const password = ref('')
const message = ref('')

const login = async () => {
  if (!username.value || !password.value) {
    message.value = 'Please enter both username and password.'
    return
  }
  try {
    const res = await axios.post('http://localhost:5280/api/Auth/login', {
      username: username.value,
      password: password.value,
    })

    // Store token 
    localStorage.setItem('token', res.data.token)

    message.value = res.data || 'Login successful!'
    router.push('/notelist')
  } catch (err: any) {
    if (err?.response?.status === 401) {
      message.value = 'Incorrect username or password.'
    } else {
      message.value = err?.response?.data || 'Login failed'
    }
  }
}
</script>

<style scoped>
@keyframes fade-in {
  from { opacity: 0; }
  to { opacity: 1; }
}
@keyframes fade-in-up {
  from { opacity: 0; transform: translateY(40px); }
  to { opacity: 1; transform: translateY(0); }
}
.animate-fade-in {
  animation: fade-in 0.8s ease;
}
.animate-fade-in-up {
  animation: fade-in-up 0.8s cubic-bezier(0.4,0,0.2,1);
}
</style>

