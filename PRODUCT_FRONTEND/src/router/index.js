import { createRouter, createWebHistory } from "vue-router";
import Home from "../pages/Home.vue";
import ProductDetails from "../pages/ProductDetails.vue";
import Add from "../pages/Add.vue";

const routes = [
  { path: "/", component: Home },
  { path: "/add", component: Add }, // Define the Add route
  {
    path: "/details/:id",
    name: "ProductDetails",
    component: ProductDetails,
    props: true, // Pass route parameters as props to the component
  },
  { path: "/:pathMatch(.*)*", redirect: "/" },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
