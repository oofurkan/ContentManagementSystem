# CMS Frontend - React Application

A React frontend application that integrates with your ASP.NET Core Web API for Content Management System.

## Features

- 🔐 **Authentication**: Login with username/password, JWT token storage
- 🛡️ **Protected Routes**: Secure access to content management features
- 📋 **Content List**: View and filter contents with language and category filters
- ➕ **Add Content**: Create new content with all required fields
- 🎨 **Modern UI**: Clean, responsive design with CSS Grid and Flexbox
- 🔄 **Auto Token Management**: Automatic JWT token handling with Axios interceptors

## Prerequisites

- Node.js (version 14 or higher)
- npm or yarn
- Your ASP.NET Core Web API running on `http://localhost:5000`

## Installation

1. **Navigate to the project directory:**
   ```bash
   cd react-cms-frontend
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   npm start
   ```

The application will open in your browser at `http://localhost:3000`.

## Project Structure

```
react-cms-frontend/
├── public/
│   └── index.html
├── src/
│   ├── components/
│   │   ├── AddContentForm.js      # Form to create new content
│   │   ├── ContentList.js         # Display and filter contents
│   │   ├── Home.js               # Welcome page
│   │   ├── LoginForm.js          # Authentication form
│   │   ├── Navbar.js             # Navigation component
│   │   └── ProtectedRoute.js     # Route protection wrapper
│   ├── context/
│   │   └── AuthContext.js        # Authentication state management
│   ├── services/
│   │   └── api.js               # Axios configuration and API calls
│   ├── App.js                   # Main application component
│   ├── index.js                 # Application entry point
│   └── index.css                # Global styles
├── package.json
└── README.md
```

## API Integration

The frontend integrates with your ASP.NET Core Web API endpoints:

### Authentication
- `POST /api/auth/login` - User login

### Content Management
- `GET /api/Content/filter` - Get filtered contents
- `POST /api/Content` - Create new content
- `GET /api/Content/{contentId}/user/{userId}` - Get specific content
- `POST /api/Content/category` - Create content category

### User Management
- `POST /api/User` - Create new user
- `GET /api/User/{userId}/contents` - Get user's contents

## Usage

### 1. Login
- Navigate to the login page
- Enter your username and password
- Upon successful login, you'll be redirected to the content list

### 2. View Content
- Access the "Content List" page
- Use filters to search by language or category
- Contents are displayed in a responsive grid layout

### 3. Add Content
- Click "Add Content" in the navigation
- Fill in all required fields:
  - Title (max 200 characters)
  - Description
  - Language (max 10 characters)
  - Image URL
  - Category ID (optional)
  - User ID (optional)
  - Content Variants (at least one required)
- Submit the form to create new content

### 4. Logout
- Click the "Logout" button in the navigation
- This will clear your JWT token and redirect to the login page

## Configuration

### API Base URL
The API base URL is configured in `src/services/api.js`:
```javascript
const api = axios.create({
  baseURL: 'http://localhost:5000/api',
  // ...
});
```

### CORS
Make sure your ASP.NET Core API has CORS configured to allow requests from `http://localhost:3000`.

## Development

### Available Scripts

- `npm start` - Start development server
- `npm build` - Build for production
- `npm test` - Run tests
- `npm eject` - Eject from Create React App

### Key Features Implementation

1. **Authentication Context**: Manages user authentication state across the application
2. **Protected Routes**: Automatically redirects unauthenticated users to login
3. **Axios Interceptors**: Automatically adds JWT tokens to requests and handles 401 errors
4. **Form Validation**: Client-side validation for all form inputs
5. **Error Handling**: Comprehensive error handling with user-friendly messages

## Troubleshooting

### Common Issues

1. **CORS Errors**: Ensure your API has CORS configured for `http://localhost:3000`
2. **Authentication Failures**: Check that your API's login endpoint returns the expected JWT token format
3. **API Connection**: Verify your ASP.NET Core API is running on `http://localhost:5000`

### Development Tips

- Use browser developer tools to inspect network requests
- Check the browser console for JavaScript errors
- Verify API responses match the expected data structure

## Production Deployment

1. Build the application:
   ```bash
   npm run build
   ```

2. Deploy the `build` folder to your web server

3. Update the API base URL in `src/services/api.js` to point to your production API

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is part of the Content Management System and follows the same license as the main project.
