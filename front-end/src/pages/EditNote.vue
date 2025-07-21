<template>
  <div class="min-h-screen flex flex-col items-center justify-center bg-white py-8">
    <div class="bg-white border border-gray-200 shadow-2xl rounded-3xl px-8 py-12 w-full max-w-xl animate-fade-in-up">
      <h1 class="text-3xl font-extrabold text-black mb-8 text-center tracking-tight flex items-center justify-center gap-2">📝 Edit Note</h1>

      <form @submit.prevent="updateNote" class="space-y-8">
        <div>
          <label class="block text-base font-semibold mb-2 text-gray-700">Title</label>
          <input
            v-model="title"
            type="text"
            placeholder="Note Title"
            class="w-full px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition"
            required
          />
        </div>

        <div>
          <label class="block text-base font-semibold mb-2 text-gray-700">Content</label>
          <textarea
            v-model="content"
            placeholder="Write something awesome..."
            class="w-full h-40 px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm resize-none transition"
          ></textarea>
        </div>

        <div class="flex justify-end gap-4 mt-8">
          <button
            type="button"
            @click="goBack"
            class="px-5 py-3 rounded-xl bg-white text-black font-semibold border border-gray-400 shadow hover:bg-gray-100 transition"
          >
            Cancel
          </button>
          <button
            type="submit"
            class="px-5 py-3 rounded-xl bg-black text-white font-semibold shadow hover:bg-gray-800 transition"
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
