import axios from "axios";
import { useToast } from "vue-toastification";

const toast = useToast();

// Create an Axios instance with default configurations
const apiClient = axios.create({
  baseURL: "http://localhost:5074/api/products", // Base URL for all API requests
  timeout: 10000, // Set timeout for requests (in milliseconds)
});

// Centralized error handling function
const handleApiError = (error) => {
  if (error.response) {
    // Handle errors where the server responds with a status code outside 2xx
    switch (error.response.status) {
      case 404:
        toast.error("Resource not found. Please try again.");
        break;
      case 500:
        toast.error("Internal server error. Please try again later.");
        break;
      case 503:
        toast.error("Service is unavailable. Please try again later.");
        break;
      default:
        // Display specific server error message if available, or a generic message
        toast.error(
          error.response.data.message ||
            "Something went wrong. Please try again."
        );
        break;
    }
  } else if (error.request) {
    // Handle cases where no response is received from the server
    toast.error(
      "Unable to connect to the server. Please check your connection."
    );
  } else {
    toast.error("An unexpected error occurred. Please try again later.");
  }

  throw error;
};

// Add an Axios response interceptor to handle all responses globally
apiClient.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    // Handle errors centrally
    const errorMessage =
      error.response?.data?.message || "An unexpected error occurred.";
    toast.error(errorMessage);
    return Promise.reject(error);
  }
);

// Export API methods for use across the application
export default {
  // Fetch products with optional pagination, search, and sorting
  getProducts(page, pageSize, search = "", sortBy = "name", ascending = true) {
    return apiClient.get(
      `?page=${page}&pageSize=${pageSize}&search=${search}&sortBy=${sortBy}&ascending=${ascending}`
    );
  },
  // Fetch a single product by its ID
  getProduct(id) {
    return apiClient.get(`/${id}`);
  },
  // Add a new product
  async addProduct(product) {
    try {
      const response = await apiClient.post("", product); // POST request to create a product
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  // Update an existing product by its ID
  async updateProduct(id, product) {
    try {
      const response = await apiClient.put(`/${id}`, product); // PUT request to update a product
      return response.data;
    } catch (error) {
      throw error;
    }
  },
  // Delete a product by its ID
  async deleteProduct(id) {
    try {
      const response = await apiClient.delete(`/${id}`); // DELETE request to remove a product
      return response.data;
    } catch (error) {
      throw error;
    }
  },
};
