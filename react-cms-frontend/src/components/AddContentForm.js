import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { contentAPI } from '../services/api';

const AddContentForm = () => {
  const [formData, setFormData] = useState({
    title: '',
    description: '',
    language: '',
    imageUrl: '',
    categoryId: '',
    userId: '',
    variants: [''],
  });
  const [categories, setCategories] = useState([]);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const navigate = useNavigate();

  // Sayfa yüklendiğinde kategoriler ve kullanıcıları çek
  useEffect(() => {
    const fetchData = async () => {
      try {
        const categoriesRes = await contentAPI.getCategories();
        setCategories(categoriesRes.data);

        const usersRes = await contentAPI.getUsers();
        setUsers(usersRes.data);
      } catch (err) {
        console.error('Failed to fetch categories or users', err);
      }
    };

    fetchData();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleVariantChange = (index, value) => {
    const newVariants = [...formData.variants];
    newVariants[index] = value;
    setFormData(prev => ({
      ...prev,
      variants: newVariants,
    }));
  };

  const addVariant = () => {
    setFormData(prev => ({
      ...prev,
      variants: [...prev.variants, ''],
    }));
  };

  const removeVariant = (index) => {
    if (formData.variants.length > 1) {
      const newVariants = formData.variants.filter((_, i) => i !== index);
      setFormData(prev => ({
        ...prev,
        variants: newVariants,
      }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      const filteredVariants = formData.variants.filter(variant => variant.trim() !== '');
      const contentData = {
        ...formData,
        variants: filteredVariants,
      };

      await contentAPI.createContent(contentData);
      setSuccess('Content created successfully!');

      setFormData({
        title: '',
        description: '',
        language: '',
        imageUrl: '',
        categoryId: '',
        userId: '',
        variants: [''],
      });

      setTimeout(() => {
        navigate('/content');
      }, 2000);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create content. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <h1>Add New Content</h1>

      {error && <div className="alert alert-error">{error}</div>}
      {success && <div className="alert alert-success">{success}</div>}

      <div className="card">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="title">Title *</label>
            <input
              type="text"
              id="title"
              name="title"
              className="form-control"
              value={formData.title}
              onChange={handleChange}
              maxLength={200}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="description">Description *</label>
            <textarea
              id="description"
              name="description"
              className="form-control"
              value={formData.description}
              onChange={handleChange}
              rows={4}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="language">Language *</label>
            <input
              type="text"
              id="language"
              name="language"
              className="form-control"
              value={formData.language}
              onChange={handleChange}
              maxLength={10}
              placeholder="e.g., en, tr, es"
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="imageUrl">Image URL *</label>
            <input
              type="url"
              id="imageUrl"
              name="imageUrl"
              className="form-control"
              value={formData.imageUrl}
              onChange={handleChange}
              placeholder="https://example.com/image.jpg"
              required
            />
          </div>

          {/* Kategori Dropdown */}
          <div className="form-group">
            <label htmlFor="categoryId">Category *</label>
            <select
              id="categoryId"
              name="categoryId"
              className="form-control"
              value={formData.categoryId}
              onChange={handleChange}
              required
            >
              <option value="">Select a category</option>
              {categories.map(cat => (
                <option key={cat.id} value={cat.id}>{cat.name}</option>
              ))}
            </select>
          </div>

          {/* Kullanıcı Dropdown */}
          <div className="form-group">
            <label htmlFor="userId">User *</label>
            <select
              id="userId"
              name="userId"
              className="form-control"
              value={formData.userId}
              onChange={handleChange}
              required
            >
              <option value="">Select a user</option>
              {users.map(u => (
                <option key={u.id} value={u.id}>{u.name}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Content Variants *</label>
            {formData.variants.map((variant, index) => (
              <div key={index} style={{ display: 'flex', gap: '10px', marginBottom: '10px' }}>
                <input
                  type="text"
                  className="form-control"
                  value={variant}
                  onChange={(e) => handleVariantChange(index, e.target.value)}
                  placeholder={`Variant ${index + 1}`}
                  required
                />
                {formData.variants.length > 1 && (
                  <button
                    type="button"
                    onClick={() => removeVariant(index)}
                    className="btn btn-danger"
                    style={{ padding: '8px 12px' }}
                  >
                    Remove
                  </button>
                )}
              </div>
            ))}
            <button
              type="button"
              onClick={addVariant}
              className="btn btn-secondary"
              style={{ marginTop: '10px' }}
            >
              Add Variant
            </button>
          </div>

          <div style={{ display: 'flex', gap: '15px', marginTop: '20px' }}>
            <button type="submit" className="btn btn-primary" disabled={loading}>
              {loading ? 'Creating...' : 'Create Content'}
            </button>
            <button type="button" onClick={() => navigate('/content')} className="btn btn-secondary">
              Cancel
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddContentForm;
