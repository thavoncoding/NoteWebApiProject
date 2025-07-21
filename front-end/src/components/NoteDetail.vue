<template>
  <div class="p-4">
    <div v-if="note">
      <h1 class="text-xl font-bold mb-2">{{ note.title }}</h1>
      <p class="mb-4">{{ note.content }}</p>
      <p class="text-sm text-gray-500">Created at: {{ formatDate(note.createdAt) }}</p>

      <div class="mt-4 flex gap-2">
        <button @click="goBack" class="bg-gray-300 px-4 py-2 rounded">Back</button>
        <button @click="editNote" class="bg-yellow-400 px-4 py-2 rounded">Edit</button>
        <button @click="deleteNote" class="bg-red-500 text-white px-4 py-2 rounded">Delete</button>
      </div>
    </div>
    <div v-else>
      <p>Loading...</p>
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
  router.push('/')
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
