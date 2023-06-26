import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const routes =
[
    {
        path: '/',
        name: 'home',
        component: HomeView
    },
    {
        path: '/texts/:id',
        name: 'text',
        component: () => import('../views/ReadTextView.vue'),
        props: true
    }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
