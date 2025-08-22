import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar = () => {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="navbar">
      <div className="container">
        <div className="navbar-content">
          <Link to="/" className="navbar-brand">
            CMS Frontend
          </Link>
          <ul className="navbar-nav">
            {isAuthenticated ? (
              <>
                <li>
                  <Link to="/content">Content List</Link>
                </li>
                <li>
                  <Link to="/content/add">Add Content</Link>
                </li>
                <li>
                  <button 
                    onClick={handleLogout}
                    className="btn btn-secondary"
                    style={{ background: 'none', border: 'none', color: 'white' }}
                  >
                    Logout
                  </button>
                </li>
              </>
            ) : (
              <>
                <li>
                  <Link to="/login">Login</Link>
                </li>
                <li>
                  <Link to="/register">Register</Link>
                </li>
              </>
            )}
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
