import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Home = () => {
  const { isAuthenticated } = useAuth();

  return (
    <div className="container">
      <div className="card">
        <h1>Welcome to CMS Frontend</h1>
        <p>This is a React frontend application that integrates with your ASP.NET Core Web API.</p>
        
        {isAuthenticated ? (
          <div>
            <p>You are logged in. You can now:</p>
            <ul>
              <li><Link to="/content">View Content List</Link></li>
              <li><Link to="/content/add">Add New Content</Link></li>
            </ul>
          </div>
        ) : (
          <div>
            <p>Please log in to access the content management features.</p>
            <div style={{ display: 'flex', gap: '15px', justifyContent: 'center' }}>
              <Link to="/login" className="btn btn-primary">
                Login
              </Link>
              <Link to="/register" className="btn btn-secondary">
                Register
              </Link>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Home;
