import { createRouter, createWebHistory } from 'vue-router'
import NoteList from '../features/notes/NoteList.vue'
import NoteDetail from '../features/notes/NoteDetail.vue'
import EditNote from '../features/notes/EditNote.vue'
import Login from '../features/auth/Login.vue'
import Register from '../features/auth/Register.vue'

const routes = [
  { path: '/', component: Login },
  { path: '/register', component: Register },
  { path: '/notelist', name: 'NoteList', component: NoteList, meta: {requiresAuth: true} },
  { path: '/note/:id', name: 'NoteDetail', component: NoteDetail, meta: {requiresAuth: true} },
  { path: '/edit/:id' , name: 'EditNote', component: EditNote, meta: {requiresAuth: true}},
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

// Global navigation guard
router.beforeEach((to, _from, next) => {
  const isAuthenticated = !!localStorage.getItem('token')
  if (to.meta.requiresAuth && !isAuthenticated) {
    next('/') // Redirect to login
  } else {
    next()
  }
})

export default router