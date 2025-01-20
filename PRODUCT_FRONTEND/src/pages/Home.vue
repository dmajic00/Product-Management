<template>
  <div>
    <ProductList
      ref="productListRef"
      @edit="editProduct"
      @delete="confirmDelete"
    />
    <ConfirmModal
      v-if="isModalVisible"
      :visible="isModalVisible"
      title="Confirm Deletion"
      message="Are you sure you want to delete this product?"
      @confirm="deleteConfirmed"
      @cancel="cancelDelete"
    />
    <EditModal
      v-if="isEditModalVisible"
      :visible="isEditModalVisible"
      title="Edit Product"
      :product="editedProduct"
      @save="saveEdit"
      @close="closeEditModal"
    />
  </div>
</template>

<script>
import { ref } from "vue";
import { useToast } from "vue-toastification";
import ProductList from "../components/ProductList.vue";
import ConfirmModal from "../components/ConfirmModal.vue";
import EditModal from "../components/EditModal.vue";
import api from "../services/api";

export default {
  components: {
    ProductList,
    ConfirmModal,
    EditModal,
  },
  setup() {
    const toast = useToast();
    const productListRef = ref(null);
    const isModalVisible = ref(false);
    const isEditModalVisible = ref(false);
    const productToDelete = ref(null);
    const currentProduct = ref(null);

    const refreshProductList = () => {
      if (productListRef.value) {
        productListRef.value.fetchProducts(); // Call fetchProducts on ProductList
      } else {
        console.error("ProductList ref is undefined.");
      }
    };

    const editedProduct = ref({}); // Temporary product for editing

    const editProduct = (product) => {
      currentProduct.value = product; // Reference the original product
      editedProduct.value = { ...product }; // Create a copy for editing
      isEditModalVisible.value = true;
    };

    const saveEdit = async () => {
      try {
        await api.updateProduct(
          editedProduct.value.productId,
          editedProduct.value
        );
        toast.success("Product updated successfully.");
        refreshProductList(); // Refresh the list
        closeEditModal(); // Close the modal after saving
      } catch (err) {
        if (err.response?.status === 409) {
          toast.clear(); // Clear any previous toasts
          toast.error(err.response.data.message); // Handle conflict
        } else {
          toast.error("Failed to update product.");
        }
      }
    };

    const closeEditModal = () => {
      isEditModalVisible.value = false;
      editedProduct.value = {}; // Reset the edited product
    };

    const confirmDelete = (product) => {
      productToDelete.value = product;
      isModalVisible.value = true;
    };

    const deleteConfirmed = async () => {
      try {
        await api.deleteProduct(productToDelete.value.productId);
        toast.success("Product deleted successfully.");
        refreshProductList(); // Refresh the list
        isModalVisible.value = false;
        productToDelete.value = null;
      } catch (err) {
        const errorMessage =
          err?.response?.data?.message || "Failed to delete product.";
        toast.error(errorMessage);
      }
    };

    const cancelDelete = () => {
      isModalVisible.value = false;
      productToDelete.value = null;
    };

    return {
      productListRef,
      isModalVisible,
      isEditModalVisible,
      productToDelete,
      currentProduct,
      editedProduct,
      refreshProductList,
      saveEdit,
      confirmDelete,
      deleteConfirmed,
      cancelDelete,
      editProduct,
      closeEditModal,
    };
  },
};
</script>
