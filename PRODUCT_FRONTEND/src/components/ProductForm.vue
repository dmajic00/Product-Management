<template>
  <div>
    <h1>Add Product</h1>
    <!-- Form for adding a product -->
    <form @submit.prevent="submitForm">
      <!-- Product Name Input -->
      <input
        v-model="product.name"
        placeholder="Name"
        required
        :disabled="isSubmitting"
      />

      <!-- Product Price Input -->
      <input
        v-model.number="product.price"
        placeholder="Price"
        type="number"
        required
        :disabled="isSubmitting"
        min="0"
        <!--
        Ensure
        price
        is
        non-negative
        --
      />
      />

      <!-- Product Description Input -->
      <textarea
        v-model="product.description"
        placeholder="Description"
        :disabled="isSubmitting"
      ></textarea>

      <!-- Product Quantity Input -->
      <input
        v-model.number="product.quantity"
        placeholder="Quantity"
        type="number"
        required
        :disabled="isSubmitting"
        min="0"
      />

      <!-- Submit Button -->
      <button type="submit" :disabled="isSubmitting">
        {{ isSubmitting ? "Saving..." : "Save" }}
        <!-- Indicate saving state -->
      </button>
    </form>
  </div>
</template>

<script>
import { ref } from "vue";
import { useToast } from "vue-toastification";
import api from "../services/api";

export default {
  setup() {
    const toast = useToast(); // Toast notifications for user feedback
    const isSubmitting = ref(false); // Track form submission state
    const product = ref({ name: "", price: 0, description: "", quantity: 0 }); // Product data

    // Handle form submission
    const submitForm = async () => {
      if (isSubmitting.value) return; // Prevent duplicate submissions
      isSubmitting.value = true; // Set submitting state

      try {
        // Call API to add the product
        await api.addProduct(product.value);
        toast.success("Product added successfully."); // Show success notification
        product.value = { name: "", price: 0, description: "", quantity: 0 }; // Reset form
        this.$router.push("/"); // Navigate to product list page
      } catch (error) {
        // Handle different types of errors
        if (error.response?.status === 409) {
          toast.error("A product with this name already exists."); // Handle conflict error
        } else if (error.request) {
          toast.error(
            "Unable to connect to the server. Please check your connection."
          ); // Handle connection issues
        } else {
          toast.error("An unexpected error occurred. Please try again."); // General error
        }
      } finally {
        isSubmitting.value = false; // Re-enable form submission
      }
    };

    return {
      product,
      isSubmitting,
      submitForm,
    };
  },
};
</script>

<style>
form {
  display: flex;
  flex-direction: column;
  gap: 15px;
  max-width: 400px;
  margin: auto;
}

input,
textarea,
button {
  padding: 10px;
  font-size: 16px;
  border-radius: 5px;
  border: 1px solid #ddd;
}

button {
  background-color: #4caf50;
  color: white;
  border: none;
  cursor: pointer;
}

button:disabled {
  background-color: #aaa;
  cursor: not-allowed;
}
</style>
