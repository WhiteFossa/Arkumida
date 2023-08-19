import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from "@/views/LoginView.vue";
import ReadTextView from "@/views/ReadTextView.vue";

const routes =
[
    // Main page
    {
        path: '/',
        name: 'home',
        component: HomeView
    },

    // Read text
    {
        path: '/texts/:id/page/:page',
        name: 'text',
        component: ReadTextView,
        props: true
    },

    // Login
    {
        path: '/login',
        name: 'login',
        component: LoginView
    }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
