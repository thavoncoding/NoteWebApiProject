<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-950 text-white">
    <div class="w-full max-w-xl bg-gray-900 p-8 rounded-xl shadow-lg">
      <h1 class="text-3xl font-bold mb-6 text-center">📝 Edit Note</h1>

      <form @submit.prevent="updateNote" class="space-y-6">
        <div>
          <label class="block text-sm mb-1">Title</label>
          <input
            v-model="title"
            type="text"
            placeholder="Note Title"
            class="w-full p-3 bg-gray-800 border border-gray-700 rounded-lg focus:outline-none focus:ring-2 focus:ring-yellow-400"
            required
          />
        </div>

        <div>
          <label class="block text-sm mb-1">Content</label>
          <textarea
            v-model="content"
            placeholder="Write something awesome..."
            class="w-full h-40 p-3 bg-gray-800 border border-gray-700 rounded-lg resize-none focus:outline-none focus:ring-2 focus:ring-yellow-400"
          ></textarea>
        </div>

        <div class="flex justify-end gap-3">
          <button
            type="button"
            @click="goBack"
            class="px-5 py-2 rounded-lg bg-gray-600 hover:bg-gray-500 transition"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-5 py-2 rounded-lg bg-yellow-400 text-black font-semibold hover:bg-yellow-300 transition"
          >
            Update
          </button>
        </div>
      </form>
    </div>
  </div>
</template>


<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import axios from 'axios'

const route = useRoute()
const router = useRouter()

const title = ref('')
const content = ref('')
const id = Number(route.params.id)

const fetchNote = async () => {
  try {
    const res = await axios.get(`http://localhost:5280/api/Notes/${id}`)
    title.value = res.data.title
    content.value = res.data.content
  } catch (err) {
    console.error('Failed to load note', err)
    alert('Failed to load note.')
  }
}

const updateNote = async () => {
  try {
    await axios.put(`http://localhost:5280/api/Notes/${id}`, {
      title: title.value,
      content: content.value,
    })
    router.push(`/note/${id}`)
  } catch (err) {
    console.error('Failed to update note:', err)
    alert('Failed to update note.')
  }
}

const goBack = () => {
  router.push(`/note/${id}`)
}

onMounted(fetchNote)
</script>
