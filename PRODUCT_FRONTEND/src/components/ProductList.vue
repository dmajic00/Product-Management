<template>
  <div>
    <!-- Search Bar and Sorting -->
    <div class="controls">
      <input
        v-model="searchQuery"
        @input="fetchProducts"
        type="text"
        placeholder="Search by name or price..."
        class="search-bar"
      />
      <div class="sort-container">
        <i class="fas fa-filter"></i>
        <select id="sort" v-model="sortBy" @change="fetchProducts">
          <option value="name">Name</option>
          <option value="price">Price</option>
        </select>
        <button class="sort-order" @click="toggleSortOrder">
          <i :class="ascending ? 'fas fa-arrow-up' : 'fas fa-arrow-down'"></i>
        </button>
      </div>
    </div>

    <!-- Product List -->
    <div class="product-list">
      <div
        v-if="products.length > 0"
        v-for="product in products"
        :key="product.productId"
        class="product-item"
      >
        <div class="product-info" @click="navigateToDetails(product.productId)">
          <p>
            <strong>{{ product.name }}</strong>
          </p>
          <p>{{ formatPrice(product.price) }}</p>
        </div>
        <div class="product-details">
          <i
            class="fas fa-info-circle details-icon"
            @click="navigateToDetails(product.productId)"
          ></i>
        </div>
        <div class="product-actions">
          <button class="btn-edit" @click.stop="editProduct(product)">
            Edit
          </button>
          <button class="btn-delete" @click.stop="deleteProduct(product)">
            Delete
          </button>
        </div>
      </div>

      <!-- Display Message When No Products Found -->
      <div v-else class="no-products-message">
        No product matching that name or price
      </div>
    </div>

    <!-- Pagination -->
    <div class="pagination">
      <button :disabled="page === 1" @click="changePage(page - 1)">
        Previous
      </button>
      <span>Page {{ page }} of {{ totalPages }}</span>
      <button :disabled="page === totalPages" @click="changePage(page + 1)">
        Next
      </button>
    </div>
  </div>
</template>

<script>
import { useToast } from "vue-toastification";
import api from "../services/api";

export default {
  props: {
    initialPage: { type: Number, default: 1 },
    initialPageSize: { type: Number, default: 10 },
  },
  data() {
    return {
      products: [],
      page: this.initialPage,
      pageSize: this.initialPageSize,
      totalProducts: 0,
      totalPages: 0,
      searchQuery: "",
      sortBy: "name",
      ascending: true,
    };
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await api.getProducts(
          this.page,
          this.pageSize,
          this.searchQuery,
          this.sortBy,
          this.ascending
        );
        this.products = response.data;
        const totalCount = response.headers["x-total-count"];
        if (totalCount) {
          this.totalProducts = parseInt(totalCount, 10);
          this.totalPages = Math.ceil(this.totalProducts / this.pageSize);
        }
      } catch (err) {
        const toast = useToast();
        toast.error("Failed to load products.");
      }
    },
    formatPrice(price) {
      return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "EUR",
      }).format(price);
    },
    navigateToDetails(productId) {
      this.$router.push(`/details/${productId}`);
    },
    editProduct(product) {
      this.$emit("edit", product);
    },
    deleteProduct(product) {
      this.$emit("delete", product);
    },
    toggleSortOrder() {
      this.ascending = !this.ascending;
      this.fetchProducts();
    },
    changePage(newPage) {
      this.page = newPage;
      this.fetchProducts();
    },
  },
  mounted() {
    this.fetchProducts();
  },
};
</script>

<style>
.product-list {
  display: flex;
  flex-direction: column;
  margin-top: 100px;
  gap: 20px;
  width: 500px;
}

.product-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 30px;
  background-color: #ececec;
  border: 2px solid #b68d40;
  border-radius: 4px;
  cursor: pointer;
}

.product-item:hover {
  background-color: #f0e8d0;
}

.product-info {
  display: flex;
  flex-direction: column;
  justify-content: space-around;

  padding: 10px;
}

.product-info p {
  margin: 20px;
  font-size: 25px;
}

.product-actions {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.product-details {
  display: flex;
  padding: 20px;
  background: #fff;
  border: 1px solid #ddd;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

button {
  padding: 8px 12px;
  border: none;
  border-radius: 4px;
}

.clickable-row {
  cursor: pointer;
}

.controls {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.search-bar {
  margin-top: 100px;
  flex: 1;
  max-width: 300px;
  border: solid 1px;
  border-radius: 4px;
  font-size: 16px;
  margin-right: 10px;
}

.no-products-message {
  text-align: center;
  color: #b68d40;
  font-size: 1.2rem;
  margin-top: 20px;
}

.sort-container {
  margin-top: 100px;
  display: flex;
  align-items: center;
  gap: 10px;
}

.fa-filter {
  color: #d6ad60;
}

.sort-order {
  background: #d6ad60;
  border: none;
  cursor: pointer;
  font-size: 0.6rem;
}
#sort {
  border: solid 1px;
  border-radius: 4px;
  font-size: 16px;
}
.pagination {
  width: 100%;
  position: relative;
  bottom: 0;
  left: 0;
  background-color: #ececec;
  margin-top: 20px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 10px;
}
</style>
