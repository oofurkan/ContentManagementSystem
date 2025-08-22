import React, { useState, useEffect } from 'react';
import { contentAPI } from '../services/api';

const ContentList = () => {
  const [contents, setContents] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [filters, setFilters] = useState({
    language: '',
    categoryName: '',
  });

  useEffect(() => {
    fetchContents();
  }, [filters]);

  const fetchContents = async () => {
    try {
      setLoading(true);
      const response = await contentAPI.getFilteredContents(filters);
      setContents(response.data);
    } catch (err) {
      setError('Failed to fetch contents. Please try again.');
      console.error('Error fetching contents:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  if (loading) {
    return <div className="loading">Loading contents...</div>;
  }

  if (error) {
    return (
      <div className="container">
        <div className="alert alert-error">
          {error}
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <h1>Content List</h1>
      
      {/* Filters */}
      <div className="card">
        <h3>Filters</h3>
        <div style={{ display: 'flex', gap: '15px', flexWrap: 'wrap' }}>
          <div className="form-group" style={{ flex: '1', minWidth: '200px' }}>
            <label htmlFor="language">Language</label>
            <input
              type="text"
              id="language"
              name="language"
              className="form-control"
              value={filters.language}
              onChange={handleFilterChange}
              placeholder="Filter by language"
            />
          </div>
          <div className="form-group" style={{ flex: '1', minWidth: '200px' }}>
            <label htmlFor="categoryName">Category</label>
            <input
              type="text"
              id="categoryName"
              name="categoryName"
              className="form-control"
              value={filters.categoryName}
              onChange={handleFilterChange}
              placeholder="Filter by category"
            />
          </div>
        </div>
      </div>

      {/* Content Grid */}
      {contents.length === 0 ? (
        <div className="card">
          <p>No contents found.</p>
        </div>
      ) : (
        <div className="content-grid">
          {contents.map((content) => (
            <div key={content.id} className="content-card">
              <img
                src={content.imageUrl}
                alt={content.title}
                className="content-image"
                onError={(e) => {
                  e.target.src = 'https://via.placeholder.com/300x200?text=No+Image';
                }}
              />
              <div className="content-body">
                <h3 className="content-title">{content.title}</h3>
                <p className="content-description">
                  {content.description.length > 100
                    ? `${content.description.substring(0, 100)}...`
                    : content.description}
                </p>
                <div className="content-meta">
                  <span>Language: {content.language}</span>
                  {content.categoryName && (
                    <span>Category: {content.categoryName}</span>
                  )}
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ContentList;
