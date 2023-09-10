import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from "@/views/LoginView.vue";
import ReadTextView from "@/views/ReadTextView.vue";
import ProfileView from "@/views/ProfileView.vue";
import ConfirmEmailView from "@/views/ConfirmEmailView.vue";

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
    },

    // Profile
    {
        path: '/profile/:part?/:action?',
        name: 'profile',
        component: ProfileView,
        props: true
    },

    // Email confirmation
    {
        path: '/confirmEmail',
        name: 'confirmEmail',
        component: ConfirmEmailView,
        props: true
    }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
