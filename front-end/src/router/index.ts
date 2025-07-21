import { createRouter, createWebHistory } from 'vue-router'
import NoteList from '../components/NoteList.vue'
import NoteDetail from '../components/NoteDetail.vue'
import EditNote from '../pages/EditNote.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'

const routes = [
  { path: '/login', component: LoginView },
  { path: '/register', component: RegisterView },
  { path: '/', name: 'NoteList', component: NoteList },
  { path: '/note/:id', name: 'NoteDetail', component: NoteDetail },
  { path: '/edit/:id' , name: 'EditNote', component: EditNote},
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router