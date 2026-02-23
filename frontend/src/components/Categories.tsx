import React, { useState, useEffect } from 'react';
import axios from 'axios';

enum Purpose { Expense = 'Expense', Income = 'Income', Both = 'Both' }

interface Category {
  id: number;
  description: string;
  purpose: Purpose;
}

// Componente para criação e listagem de categorias
const Categories: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState('');
  const [purpose, setPurpose] = useState<Purpose>(Purpose.Both);

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    const response = await axios.get<Category[]>('http://localhost:5000/api/categories');
    setCategories(response.data);
  };

  const handleSubmit = async () => {
    await axios.post('http://localhost:5000/api/categories', { description, purpose });
    fetchCategories();
    setDescription('');
    setPurpose(Purpose.Both);
  };

  return (
    <div>
      <h2>Categorias</h2>
      <input type="text" value={description} onChange={e => setDescription(e.target.value)} placeholder="Descrição" />
      <select value={purpose} onChange={e => setPurpose(e.target.value as Purpose)}>
        <option value="Expense">Despesa</option>
        <option value="Income">Receita</option>
        <option value="Both">Ambas</option>
      </select>
      <button onClick={handleSubmit}>Criar</button>
      <ul>
        {categories.map(c => (
          <li key={c.id}>{c.description} ({c.purpose})</li>
        ))}
      </ul>
    </div>
  );
};

export default Categories;