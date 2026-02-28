// Categories.tsx ajustado para depuração
import React, { useState, useEffect } from 'react';
import axios from 'axios';

enum Purpose { Expense = 'Expense', Income = 'Income', Both = 'Both' }

interface Category {
  id: number;
  description: string;
  purpose: Purpose;
}

const Categories: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState('');
  const [purpose, setPurpose] = useState<Purpose>(Purpose.Both);

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      const response = await axios.get<Category[]>('http://localhost:5054/api/categories');
      setCategories(response.data);
    } catch (error) {
      console.error('Erro ao buscar categorias:', error);
      alert('Falha ao carregar categorias.');
    }
  };

  const handleSubmit = async () => {
    if (!description.trim()) {
      alert('Descrição é obrigatória.');
      return;
    }
    try {
      // Enviar com nomes em maiúscula para match exato com DTO
      await axios.post('http://localhost:5054/api/categories', { Description: description, Purpose: purpose });
      fetchCategories();
      setDescription('');
      setPurpose(Purpose.Both);
    } catch (error: any) {
      console.error('Erro ao criar categoria:', error);
      const errorMsg = error.response?.data?.errors ? JSON.stringify(error.response.data.errors) : error.message;
      alert(`Falha ao criar categoria: ${errorMsg}`);
    }
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
      {categories.length === 0 ? (
        <p>Nenhuma categoria cadastrada.</p>
      ) : (
        <ul>
          {categories.map(c => (
            <li key={c.id}>{c.description} ({c.purpose})</li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Categories;