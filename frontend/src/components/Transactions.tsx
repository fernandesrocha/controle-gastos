import React, { useState, useEffect } from 'react';
import axios from 'axios';

enum TransactionType { Expense = 'Expense', Income = 'Income' }

interface Transaction {
  id: number;
  description: string;
  value: number;
  type: TransactionType;
  categoryId: number;
  personId: number;
}

interface Person { id: number; name: string; age: number; }
interface Category { id: number; description: string; purpose: string; }

// Componente para criação e listagem de transações
const Transactions: React.FC = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [persons, setPersons] = useState<Person[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [description, setDescription] = useState('');
  const [value, setValue] = useState(0);
  const [type, setType] = useState<TransactionType>(TransactionType.Expense);
  const [categoryId, setCategoryId] = useState(0);
  const [personId, setPersonId] = useState(0);

  useEffect(() => {
    fetchTransactions();
    fetchPersons();
    fetchCategories();
  }, []);

  const fetchTransactions = async () => {
    try {
      const response = await axios.get<Transaction[]>('http://localhost:5054/api/transactions');
      setTransactions(response.data);
    } catch (error) {
      console.error('Erro ao buscar transações:', error);
      alert('Falha ao carregar transações.');
    }
  };

  const fetchPersons = async () => {
    try {
      const response = await axios.get<Person[]>('http://localhost:5054/api/persons');
      setPersons(response.data);
    } catch (error) {
      console.error('Erro ao buscar pessoas:', error);
      alert('Falha ao carregar pessoas.');
    }
  };

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
    if (categoryId === 0 || personId === 0) {
      alert('Selecione uma categoria e pessoa válidas.');
      return;
    }
    try {
      await axios.post('http://localhost:5054/api/transactions', { description, value, type, categoryId, personId });
      fetchTransactions();
      resetForm();
    } catch (error) {
      console.error('Erro ao criar transação:', error);
      alert('Falha ao criar transação. Verifique os campos ou regras no back-end (ex.: idade para receitas).');
    }
  };

  const resetForm = () => {
    setDescription('');
    setValue(0);
    setType(TransactionType.Expense);
    setCategoryId(0);
    setPersonId(0);
  };

  return (
    <div>
      <h2>Transações</h2>
      <input type="text" value={description} onChange={e => setDescription(e.target.value)} placeholder="Descrição" />
      <input type="number" value={value} onChange={e => setValue(parseFloat(e.target.value))} placeholder="Valor" />
      <select value={type} onChange={e => setType(e.target.value as TransactionType)}>
        <option value="Expense">Despesa</option>
        <option value="Income">Receita</option>
      </select>
      <select value={categoryId} onChange={e => setCategoryId(parseInt(e.target.value))}>
        <option value={0}>Selecione Categoria</option>
        {categories.map(c => <option key={c.id} value={c.id}>{c.description}</option>)}
      </select>
      <select value={personId} onChange={e => setPersonId(parseInt(e.target.value))}>
        <option value={0}>Selecione Pessoa</option>
        {persons.map(p => <option key={p.id} value={p.id}>{p.name}</option>)}
      </select>
      <button onClick={handleSubmit}>Criar</button>
      {transactions.length === 0 ? (
        <p>Nenhuma transação cadastrada. Crie uma para começar.</p>
      ) : (
        <ul>
          {transactions.map(t => (
            <li key={t.id}>
              {t.description} - {t.value} ({t.type}) - Cat: {t.categoryId} - Pessoa: {t.personId}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Transactions;