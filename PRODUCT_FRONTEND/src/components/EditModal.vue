<template>
  <div
    v-if="visible"
    class="modal-overlay"
    role="dialog"
    aria-labelledby="modal-title"
  >
    <div class="modal-content">
      <h2 id="modal-title">{{ title }}</h2>
      <form @submit.prevent="save">
        <label for="name">Name:</label>
        <input
          id="name"
          v-model="product.name"
          type="text"
          required
          autofocus
        />

        <label for="price">Price:</label>
        <input
          id="price"
          v-model.number="product.price"
          type="number"
          required
          min="0"
        />

        <label for="quantity">Quantity:</label>
        <input
          id="quantity"
          v-model.number="product.quantity"
          type="number"
          required
          min="0"
        />

        <label for="description">Description:</label>
        <textarea id="description" v-model="product.description"></textarea>

        <div class="modal-actions">
          <button type="submit" :disabled="isSaving">
            {{ isSaving ? "Saving..." : "Save" }}
          </button>
          <button type="button" @click="onClose" :disabled="isSaving">
            Cancel
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    visible: { type: Boolean, required: true },
    title: { type: String, default: "Edit Product" },
    product: { type: Object, required: true },
  },
  emits: ["save", "close"],
  data() {
    return {
      isSaving: false, // Track saving state
    };
  },
  watch: {
    visible(newValue) {
      if (newValue) {
        this.$nextTick(() => {
          const firstInput = this.$el.querySelector("#name");
          firstInput?.focus();
        });
      }
    },
  },
  methods: {
    async save() {
      if (this.isSaving) return;
      this.isSaving = true;
      try {
        this.$emit("save", this.product);
      } finally {
        this.isSaving = false;
      }
    },
    onClose() {
      this.$emit("close");
    },
  },
};
</script>

<style>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  max-width: 500px;
  width: 100%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
}

form {
  display: flex;
  flex-direction: column;
  max-width: 400px;
  margin: 0 auto;
}

label {
  margin-top: 10px;
}

input,
textarea {
  margin-top: 5px;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 1rem;
}

button {
  margin-top: 20px;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
}

button[type="submit"] {
  background-color: #4caf50;
  color: white;
}

button[type="button"] {
  background-color: #f44336;
  color: white;
}

button:disabled {
  background-color: #ddd;
  color: #999;
  cursor: not-allowed;
}
</style>
