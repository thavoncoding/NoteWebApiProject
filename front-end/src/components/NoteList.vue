<template>
    <!-- Search and Sort -->
    <div class="flex flex-col md:flex-row items-center justify-between gap-3">
        <input v-model="searchQuery" type="text" placeholder="Search notes..."
            class="w-full md:w-1/2 px-4 py-2 border rounded" />

        <select v-model="sortOption" class="px-3 py-2 border rounded">
            <option value="newest">Newest First</option>
            <option value="oldest">Oldest First</option>
            <option value="az">Title A–Z</option>
        </select>
    </div>

    <div class="max-w-2xl mx-auto mt-10 p-6 bg-white rounded shadow space-y-6">
        <h2 class="text-2xl font-bold text-center flex items-center justify-center gap-2">
            📝 <span>My Notes</span>
        </h2>

        <!-- Add or Edit Note -->
        <form @submit.prevent="isEditing ? updateNote() : createNote()"
            class="flex flex-col md:flex-row gap-3 items-center">
            <input v-model="newTitle" type="text" placeholder="Title" class="flex-1 px-4 py-2 border rounded w-full"
                required />
            <input v-model="newContent" type="text" placeholder="Content (optional)"
                class="flex-1 px-4 py-2 border rounded w-full" />
            <button type="submit" class="px-4 py-2 bg-black text-white rounded hover:bg-gray-800 transition">
                {{ isEditing ? 'Update' : 'Add' }} Note
            </button>
            <button v-if="isEditing" type="button" @click="cancelEdit" class="px-4 py-2 text-red-600 hover:underline">
                Cancel
            </button>
        </form>

        <!-- Notes List -->
        <div v-if="notes.length === 0" class="text-gray-500 text-center">
            No notes found.
        </div>

        <ul class="space-y-4">
            <li v-for="note in filteredNotes":key="note.id" class="p-4 border rounded hover:shadow-md transition relative">
                <router-link :to="`/note/${note.id}`" class="font-semibold text-lg text-blue-600 hover:underline">
  {{ note.title }}
</router-link>

                <p v-if="note.content" class="text-sm text-gray-700">{{ note.content }}</p>
                <p class="text-xs text-gray-500 mt-1">Created at: {{ formatDate(note.createdAt) }}</p>

                <!-- Action buttons -->
                <div class="absolute top-2 right-2 space-x-2">
                    <button @click="startEdit(note)" class="text-blue-600 text-sm hover:underline">
                        Edit
                    </button>
                    <button @click="deleteNote(note.id)" class="text-red-600 text-sm hover:underline">
                        Delete
                    </button>
                </div>
            </li>
        </ul>
    </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
const searchQuery = ref('')
const sortOption = ref('newest')


interface Note {
    id: number
    title: string
    content?: string
    createdAt: string
    updatedAt?: string
}

const notes = ref<Note[]>([])
const newTitle = ref('')
const newContent = ref('')
const isEditing = ref(false)
const editingNoteId = ref<number | null>(null)

const fetchNotes = async () => {
    try {
        const res = await axios.get('http://localhost:5280/api/Notes')
        notes.value = res.data
    } catch (err) {
        console.error('Error fetching notes:', err)
    }
}

const createNote = async () => {
    try {
        await axios.post('http://localhost:5280/api/Notes', {
            title: newTitle.value,
            content: newContent.value
        })

        newTitle.value = ''
        newContent.value = ''
        await fetchNotes()
    } catch (err) {
        console.error('Error creating note:', err)
    }
}


const deleteNote = async (id: number) => {
    const confirmDelete = confirm('Are you sure you want to delete this note?')
    if (!confirmDelete) return

    try {
        await axios.delete(`http://localhost:5280/api/Notes/${id}`)
        notes.value = notes.value.filter(n => n.id !== id)
    } catch (err) {
        console.error('Error deleting note:', err)
    }
}


const startEdit = (note: Note) => {
    isEditing.value = true
    editingNoteId.value = note.id
    newTitle.value = note.title
    newContent.value = note.content || ''
}

const updateNote = async () => {
    if (!editingNoteId.value) return
    try {
        const res = await axios.put(`http://localhost:5280/api/Notes/${editingNoteId.value}`, {
            id: editingNoteId.value,
            title: newTitle.value,
            content: newContent.value,
        })
        const index = notes.value.findIndex(n => n.id === editingNoteId.value)
        if (index !== -1) {
            notes.value[index] = res.data
        }
        await fetchNotes()
        resetForm()
    } catch (err) {
        console.error('Error updating note:', err)
    }
}

const cancelEdit = () => resetForm()

const resetForm = () => {
    newTitle.value = ''
    newContent.value = ''
    isEditing.value = false
    editingNoteId.value = null
}

const formatDate = (iso: string) =>
    new Date(iso).toLocaleString(undefined, {
        timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
    });

const filteredNotes = computed(() => {
  let result = [...notes.value]

  // Search
  if (searchQuery.value.trim()) {
    const q = searchQuery.value.toLowerCase()
    result = result.filter(note =>
      note.title.toLowerCase().includes(q) ||
      (note.content?.toLowerCase().includes(q))
    )
  }

  // Sort
  switch (sortOption.value) {
    case 'newest':
      result.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      break
    case 'oldest':
      result.sort((a, b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime())
      break
    case 'az':
      result.sort((a, b) => a.title.localeCompare(b.title))
      break
  }

  return result
})

onMounted(fetchNotes)
</script>
