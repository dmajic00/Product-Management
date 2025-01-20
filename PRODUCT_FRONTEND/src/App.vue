<template>
  <div>
    <!-- Navigation Bar -->
    <nav class="navbar" v-if="isHomePage">
      <router-link to="/add" class="btn-add">Add Product</router-link>
    </nav>

    <!-- Router View -->
    <router-view />
  </div>
</template>

<script>
import { useRoute } from "vue-router";
import { computed } from "vue";

export default {
  setup() {
    const route = useRoute(); // Get the current route object

    // Check if the current route is the home page (`/`)
    const isHomePage = computed(() => route.path === "/");

    return { isHomePage };
  },
};
</script>

<style>
body {
  background-color: #2d2d2d;
  margin: 0;
  font-family: "Roboto", sans-serif;
}

#app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
}

.navbar {
  width: 100%;
  background-color: #ffffff;
  display: flex;
  justify-content: space-between;
  padding: 10px 20px;
  position: fixed;
  top: 0;
  left: 0;
  z-index: 1000;
}

nav a {
  font-weight: 700;
  text-decoration: none;
  padding: 10px 15px;
  color: #ffffff;
  background-color: #b68d40;
  border-radius: 5px;
}

nav a.router-link-exact-active {
  background-color: #122620;
}

.btn-add {
  margin: 10px auto;
  display: block;
  padding: 10px 20px;
  background-color: #b68d40;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 20px;
}

.btn-add:hover {
  background-color: #d6ad60;
}
</style>
