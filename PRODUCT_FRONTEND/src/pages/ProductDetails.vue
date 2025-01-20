<template>
  <div class="details" v-if="product">
    <!-- Product details are displayed when product data is available -->
    <h1>PRODUCT DETAILS</h1>
    <p><strong>Name:</strong> {{ product.name }}</p>
    <p><strong>Price:</strong> {{ formatPrice(product.price) }}</p>
    <p><strong>Quantity:</strong> {{ product.quantity }}</p>
    <p><strong>Description:</strong> {{ product.description }}</p>
    <p><strong>Added:</strong> {{ formatDate(product.createdAt) }}</p>
    <p>
      <strong>Last price update:</strong> {{ formatDate(product.updatedAt) }}
    </p>

    <button @click="goBack">Back to Product List</button>
  </div>
  <div v-else>
    <p>Loading product details...</p>
  </div>
</template>

<script>
import api from "../services/api"; // Importing the API service for fetching product data

export default {
  data() {
    return {
      product: null, // Stores the details of the product fetched from the API
    };
  },
  async created() {
    // Lifecycle hook to fetch product details when the component is created
    try {
      const id = this.$route.params.id; // Retrieve the product ID from the route parameters
      const response = await api.getProduct(id); // Call API to get product details
      this.product = response.data; // Assign the fetched product details to the component's data
    } catch (err) {
      // Log error and display a user-friendly notification in case of failure
      console.error("Error fetching product details:", err);
      this.$toast?.error("Failed to load product details."); // Optional: toast for user feedback
    }
  },
  methods: {
    // Navigate back to the product list page
    goBack() {
      this.$router.push("/"); // Redirect to the homepage or product list
    },

    // Format price to display in EUR currency format
    formatPrice(price) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "EUR",
      }).format(price);
    },

    // Format a date to a readable format, with fallback if the date is null
    formatDate(date) {
      if (!date) return "Price has not been changed"; // Handle null or undefined dates
      return new Intl.DateTimeFormat("en-US", {
        year: "numeric",
        month: "long",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
        second: "2-digit",
        hour12: true,
      }).format(new Date(date));
    },
  },
};
</script>

<style>
.details {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  min-width: 600px;
  margin: 100px;
  padding: 20px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.details h1 {
  align-self: center;
}

.details p {
  padding: 10px 0;
  width: 100%;
  font-size: 20px;
  box-shadow: 8px 4px 8px 4px #b68d40;
}

button {
  align-self: center;
  padding: 10px 15px;
  background-color: #122620;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  margin-top: 20px;
}

button:hover {
  background-color: #424342;
}
</style>
