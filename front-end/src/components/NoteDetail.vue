<template>
  <div class="min-h-screen flex flex-col items-center justify-center bg-gradient-to-br from-purple-100 via-white to-purple-200 py-8">
    <div class="backdrop-blur-md bg-white/70 border border-white/40 shadow-2xl rounded-3xl px-8 py-12 w-full max-w-xl animate-fade-in-up">
      <div v-if="note">
        <h1 class="text-3xl font-extrabold text-purple-700 mb-4 text-center tracking-tight">{{ note.title }}</h1>
        <p class="mb-6 text-lg text-gray-700 text-center whitespace-pre-line">{{ note.content }}</p>
        <p class="text-xs text-gray-500 text-center mb-8">Created at: {{ formatDate(note.createdAt) }}</p>

        <div class="flex justify-center gap-4 mt-6">
          <button @click="goBack" class="px-5 py-3 rounded-xl bg-gradient-to-r from-gray-300 to-gray-400 text-gray-700 font-semibold shadow hover:scale-105 transition-transform duration-200">Back</button>
          <button @click="editNote" class="px-5 py-3 rounded-xl bg-gradient-to-r from-yellow-400 to-yellow-500 text-white font-semibold shadow hover:scale-105 transition-transform duration-200">Edit</button>
          <button @click="deleteNote" class="px-5 py-3 rounded-xl bg-gradient-to-r from-red-500 to-pink-500 text-white font-semibold shadow hover:scale-105 transition-transform duration-200">Delete</button>
        </div>
      </div>
      <div v-else>
        <p class="text-center text-gray-500 animate-fade-in">Loading...</p>
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'

const route = useRoute()
const router = useRouter()
const note = ref<any>(null)

const formatDate = (iso: string) =>
  new Date(iso).toLocaleString(undefined, {
    timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
  })

const fetchNote = async () => {
  const id = route.params.id
  try {
    const res = await axios.get(`http://localhost:5280/api/Notes/${id}`)
    console.log('Fetched note:', res.data)
    note.value = res.data
  } catch (err) {
    console.error('Error loading note', err)
    alert('Failed to load note.')
  }
}



const goBack = () => {
  router.push('/notelist')
}

const editNote = () => {
  router.push(`/edit/${route.params.id}`)
}

const deleteNote = async () => {
  if (!confirm('Are you sure you want to delete this note?')) return
  try {
    await axios.delete(`http://localhost:5280/api/Notes/${route.params.id}`)
    router.push('/')
  } catch (err) {
    console.error('Failed to delete note:', err)
  }
}

onMounted(fetchNote)
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
