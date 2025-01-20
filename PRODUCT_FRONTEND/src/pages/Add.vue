<template>
  <div class="add-product">
    <h1>ADD PRODUCT</h1>
    <form @submit.prevent="addProduct">
      <input
        v-model="product.name"
        placeholder="Name"
        required
        :disabled="isSubmitting"
      />
      <input
        v-model.number="product.price"
        placeholder="Price"
        type="number"
        required
        :disabled="isSubmitting"
      />
      <textarea
        v-model="product.description"
        placeholder="Description"
        :disabled="isSubmitting"
      ></textarea>
      <input
        v-model.number="product.quantity"
        placeholder="Quantity"
        type="number"
        required
        :disabled="isSubmitting"
      />

      <div class="form-actions">
        <button type="submit" :disabled="isSubmitting">
          {{ isSubmitting ? "Adding..." : "Add Product" }}
        </button>
        <button type="button" @click="cancelAdd">Cancel</button>
      </div>
    </form>
  </div>
</template>

<script>
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useToast } from "vue-toastification";
import api from "../services/api";

export default {
  setup() {
    const toast = useToast();
    const router = useRouter(); // Initialize the router instance
    const product = reactive({
      name: "",
      price: "",
      description: "",
      quantity: "",
    });
    const isSubmitting = ref(false);
    let conflictToastId = null; // Track the conflict toast

    const addProduct = async () => {
      if (isSubmitting.value) return; // Prevent duplicate submissions
      isSubmitting.value = true;

      try {
        product.name = product.name.trim();
        await api.addProduct(product); // Call API to add the product
        toast.success("Product added successfully.");
        router.push("/"); // Redirect to product list
        conflictToastId = null; // Reset the conflict toast ID
      } catch (err) {
        if (err.response?.status === 409) {
          // Handle conflict error (duplicate product name)
          if (!conflictToastId) {
            toast.clear();
            // Show toast only if it hasn't been shown
            conflictToastId = toast.error(
              err.response.data.message ||
                "A product with this name already exists.",
              {
                onClose: () => {
                  // Reset conflictToastId when the toast is dismissed
                  conflictToastId = null;
                },
              }
            );
          }
        } else if (err.request) {
          // No response from the server
          toast.error(
            "Unable to connect to the server. Please check your connection."
          );
        } else {
          // Other unexpected errors
          toast.error("An unexpected error occurred. Please try again later.");
        }
      } finally {
        isSubmitting.value = false; // Re-enable the button
      }
    };

    // Cancel Add Method
    const cancelAdd = () => {
      router.push("/"); // Redirect back to product list
    };

    return {
      product,
      addProduct,
      cancelAdd,
      isSubmitting,
    };
  },
};
</script>

<style>
.add-product {
  display: flex;
  flex-direction: column;
  margin: 150px;
  min-width: 500px;
  min-height: 300px;
  background-color: #b68d40;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}
.add-product h1 {
  align-self: center;
}
.add-product input,
textarea {
  margin: 20px;
  height: 40px;
}
.form-actions {
  margin: 20px;
  display: flex;
  justify-content: space-between;
  padding: 50px;
}

button {
  padding: 10px 15px;
  margin: 5px;
}
</style>
