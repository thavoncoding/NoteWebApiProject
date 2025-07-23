<template>
  <div class="min-h-screen flex flex-col items-center bg-gradient-to-br from-purple-100 via-white to-purple-200 py-8">
    <!-- Top Bar -->
    <div class="flex justify-end w-full max-w-3xl px-4 animate-fade-in">
      <button type="button" @click="logout"
        class="px-5 py-2 rounded-xl bg-red-600 text-white font-semibold shadow hover:bg-red-700 transition">
        Logout
      </button>
    </div>

    <!-- Main List -->
    <div class="bg-white border border-gray-200 shadow rounded-3xl px-8 py-12 mt-8 w-full max-w-3xl">
      <!-- Search, Sort, and Heading Row -->
      <div class="flex items-center justify-between gap-4 mb-8 w-full max-w-3xl mx-auto">
        <h2 class="text-3xl font-extrabold text-black tracking-tight flex items-center gap-2 whitespace-nowrap">
          📝 <span>My Notes</span>
        </h2>
        <div class="flex flex-1 items-center gap-2">
          <div class="relative flex-1 max-w-md">
            <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 pointer-events-none">
              <svg xmlns='http://www.w3.org/2000/svg' class='h-5 w-5' fill='none' viewBox='0 0 24 24'
                stroke='currentColor'>
                <path stroke-linecap='round' stroke-linejoin='round' stroke-width='2'
                  d='M21 21l-4.35-4.35m0 0A7.5 7.5 0 104.5 4.5a7.5 7.5 0 0012.15 12.15z' />
              </svg>
            </span>
            <input v-model="searchQuery" type="text" placeholder="Search..."
              class="w-full pl-10 pr-3 py-2 text-sm rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition" />
          </div>
          <div>
            <select v-model="sortOption"
              class="pl-3 pr-8 py-2 text-sm rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white shadow-sm transition">
              <option value="newest">Newest</option>
              <option value="oldest">Oldest</option>
              <option value="az">A–Z</option>
            </select>
          </div>
        </div>
      </div>

      <!-- Add or Edit Note -->
      <form @submit.prevent="isEditing ? updateNote() : createNote()"
        class="flex flex-col md:flex-row gap-4 items-center mb-8">
        <input v-model="newTitle" type="text" placeholder="Title"
          class="flex-1 px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition"
          required />
        <input v-model="newContent" type="text" placeholder="Description"
          class="flex-1 px-5 py-3 text-base rounded-xl border border-gray-300 focus:outline-none focus:ring-2 focus:ring-black bg-white placeholder-gray-500 shadow-sm transition" />
        <button type="submit"
          class="px-4 py-2 rounded-lg bg-black text-white font-bold shadow hover:bg-gray-800 transition w-32">
          {{ isEditing ? "Update" : "Add Note" }}
        </button>
        <button v-if="isEditing" type="button" @click="cancelEdit"
          class="px-4 py-2 rounded-lg bg-white text-black font-semibold border border-gray-400 shadow hover:bg-gray-100 transition">
          Cancel
        </button>
      </form>

      <!-- Notes List -->
      <div v-if="notes.length === 0" class="text-gray-500 text-center py-8">
        No notes found.
      </div>

      <ul class="space-y-6">
        <li v-for="note in filteredNotes" :key="note.id"
          class="p-6 border border-gray-200 rounded-2xl bg-white hover:shadow-xl transition relative">
          <router-link :to="`/note/${note.id}`" class="font-semibold text-xl text-black hover:underline">
            {{ note.title }}
          </router-link>

          <p v-if="note.content" class="text-base text-gray-700 mt-2">
            {{ note.content }}
          </p>
          <p class="text-xs text-gray-500 mt-1">
            Created at: {{ formatDate(note.createdAt) }}
          </p>

          <!-- Action buttons -->
          <div class="absolute top-4 right-4 space-x-2 flex">
            <button type="button" @click="startEdit(note)"
              class="px-4 py-2 rounded-lg bg-yellow-400 text-black font-semibold shadow hover:bg-yellow-500 transition flex items-center gap-1">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                class="lucide lucide-pencil h-4 w-4">
                <path d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z" />
              </svg>
              Edit
            </button>
            <button type="button" @click="deleteNote(note.id)"
              class="px-4 py-2 rounded-lg bg-red-500 text-white font-semibold shadow hover:bg-red-600 transition flex items-center gap-1">
              <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none"
                stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"
                class="lucide lucide-trash-2 h-4 w-4">
                <path d="M3 6h18" />
                <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6" />
                <path d="M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2" />
                <path d="M10 11v6" />
                <path d="M14 11v6" />
              </svg>
              Delete
            </button>
          </div>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import axios from "axios";
import { useRouter } from 'vue-router'
import { useToast } from 'vue-toast-notification'
const toast = useToast()
const router = useRouter()
const searchQuery = ref("");
const sortOption = ref("newest");

interface Note {
  id: number;
  title: string;
  content?: string;
  createdAt: string;
  updatedAt?: string;
}

const notes = ref<Note[]>([]);
const newTitle = ref("");
const newContent = ref("");
const isEditing = ref(false);
const editingNoteId = ref<number | null>(null);

const fetchNotes = async () => {
  try {
    const res = await axios.get("http://localhost:5280/api/Notes");
    notes.value = res.data;
  } catch (err) {
    console.error("Error fetching notes:", err);
  }
};

const createNote = async () => {
  //console.log('createNote:', newTitle.value, newContent.value); 
  try {
    await axios.post("http://localhost:5280/api/Notes", {
      title: newTitle.value,
      content: newContent.value,
    }, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    await fetchNotes();
    cancelEdit();
    toast.success('Note has been added!',{
      position: 'top'
    })
  } catch (err) {
    console.error("Error creating note:", err);
    toast.error('Failed to add note',{
      position: 'top'
    })
  }
};

const deleteNote = async (id: number) => {
  const confirmDelete = confirm("Are you sure you want to delete this note?");
  if (!confirmDelete) return;

  try {
    await axios.delete(`http://localhost:5280/api/Notes/${id}`);
    notes.value = notes.value.filter((n) => n.id !== id);
    toast.success('Note has been deleted!', { position: 'top' });
  } catch (err) {
    console.error("Error deleting note:", err);
    toast.error('Failed to delete note.', { position: 'top' });
  }
};

const startEdit = (note: Note) => {
  isEditing.value = true;
  editingNoteId.value = note.id;
  newTitle.value = note.title;
  newContent.value = note.content || "";
};

const updateNote = async () => {
  if (!editingNoteId.value) return;
  try {
    await axios.put(
      `http://localhost:5280/api/Notes/${editingNoteId.value}`,
      {
        id: editingNoteId.value,
        title: newTitle.value,
        content: newContent.value,
      }
    );
    toast.success('Note has been updated!',{
      position: 'top'
    })
    await fetchNotes();
    cancelEdit();
  } catch (err) {
    console.error("Error updating note:", err);
    toast.error('Failed to update note.!',{
      position: 'top'
    })
  }
};

const cancelEdit = () => {
  isEditing.value = false;
  editingNoteId.value = null;
  newTitle.value = "";
  newContent.value = "";
};

const formatDate = (iso: string) =>
  new Date(iso).toLocaleString(undefined, {
    timeZone: Intl.DateTimeFormat().resolvedOptions().timeZone,
  });

const filteredNotes = computed(() => {
  let result = [...notes.value];

  // Search
  if (searchQuery.value.trim()) {
    const q = searchQuery.value.toLowerCase();
    result = result.filter(
      (note) =>
        note.title.toLowerCase().includes(q) ||
        note.content?.toLowerCase().includes(q)
    );
  }

  // Sort
  switch (sortOption.value) {
    case "newest":
      result.sort(
        (a, b) =>
          new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      );
      break;
    case "oldest":
      result.sort(
        (a, b) =>
          new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
      );
      break;
    case "az":
      result.sort((a, b) => a.title.localeCompare(b.title));
      break;
  }

  return result;
});

const logout = () => {
  if (!confirm('Are you sure you want to log out?')) return;
  localStorage.removeItem('token') // if using token
  router.push('/')
}

onMounted(fetchNotes);
</script>

<style scoped>
@keyframes fade-in {
  from {
    opacity: 0;
  }

  to {
    opacity: 1;
  }
}

@keyframes fade-in-up {
  from {
    opacity: 0;
    transform: translateY(40px);
  }

  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.animate-fade-in {
  animation: fade-in 0.8s ease;
}

.animate-fade-in-up {
  animation: fade-in-up 0.8s cubic-bezier(0.4, 0, 0.2, 1);
}
</style>
