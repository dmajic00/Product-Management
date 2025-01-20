<template>
  <div
    v-if="visible"
    class="modal-overlay"
    role="dialog"
    aria-labelledby="modal-title"
    aria-describedby="modal-message"
  >
    <div class="modal-content" tabindex="0">
      <h2 id="modal-title">{{ title }}</h2>
      <p id="modal-message">
        <slot>{{ message }}</slot>
      </p>
      <div class="modal-actions">
        <button class="btn-confirm" @click="$emit('confirm')">Yes</button>
        <button class="btn-cancel" @click="$emit('cancel')">No</button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    visible: { type: Boolean, required: true },
    title: { type: String, default: "Confirm" },
    message: { type: String, default: "" }, // Use slot if no message is passed
  },
  emits: ["confirm", "cancel"],
  watch: {
    visible(newValue) {
      if (newValue) {
        this.$nextTick(() => {
          const modal = this.$el.querySelector(".modal-content");
          modal?.focus(); // Focus the modal when it opens
        });
      }
    },
  },
  mounted() {
    document.addEventListener("keydown", this.handleKeyDown);
  },
  beforeUnmount() {
    document.removeEventListener("keydown", this.handleKeyDown);
  },
  methods: {
    handleKeyDown(event) {
      if (event.key === "Escape" && this.visible) {
        this.$emit("cancel");
      }
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
  animation: fadeIn 0.3s ease-out;
}

.modal-content {
  background: white;
  padding: 20px;
  border-radius: 8px;
  max-width: 400px;
  width: 100%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  outline: none; /* Remove default focus outline */
}

.modal-actions {
  display: flex;
  justify-content: flex-end;
}

.modal-actions button {
  margin: 5px;
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.modal-actions .btn-confirm {
  background-color: #4caf50;
  color: white;
}

.modal-actions .btn-cancel {
  background-color: #f44336;
  color: white;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
</style>
