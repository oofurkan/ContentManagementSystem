import axios from 'axios';

// Create axios instance with base configuration
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add JWT token to all requests
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle authentication errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid, redirect to login
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: (credentials) => api.post('/auth/login', credentials),
  register: (userData) => api.post('/auth/register', userData),
};

// Content API
export const contentAPI = {
  // Get content details by contentId and userId
  getContentById: (contentId, userId) => 
    api.get(`/Content/${contentId}/user/${userId}`),
  
  // Get filtered list of contents
  getFilteredContents: (filters = {}) => 
    api.get('/Content/filter', { params: filters }),
  
  // Add a new content
  createContent: (contentData) => 
    api.post('/Content', contentData),
  
  // Add a new content category
  createCategory: (categoryData) => 
    api.post('/Content/category', categoryData),

  // Yeni eklenen: kategorileri çek
  getCategories: () => api.get('/Content/categories'),
};

// User API
export const userAPI = {
  // Create a new user
  createUser: (userData) => 
    api.post('/User', userData),
  
  // Get all contents of a specific user
  getUserContents: (userId) => 
    api.get(`/User/${userId}/contents`),

  getUsers: () => axios.get('/api/content/users'),
  
};

export default api;
