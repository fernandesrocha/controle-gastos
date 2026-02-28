import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface Person {
  id: number;
  name: string;
  age: number;
}

// Componente para CRUD de pessoas
const Persons: React.FC = () => {
  const [persons, setPersons] = useState<Person[]>([]);
  const [name, setName] = useState('');
  const [age, setAge] = useState(0);
  const [editingId, setEditingId] = useState<number | null>(null);

  useEffect(() => {
    fetchPersons();
  }, []);

  const fetchPersons = async () => {
    const response = await axios.get<Person[]>('http://localhost:5054/api/persons');
    setPersons(response.data);
  };

  const handleSubmit = async () => {
    if (editingId) {
      await axios.put(`http://localhost:5054/api/persons/${editingId}`, { id: editingId, name, age });
    } else {
      await axios.post('http://localhost:5054/api/persons', { name, age });
    }
    fetchPersons();
    resetForm();
  };

  const handleEdit = (person: Person) => {
    setName(person.name);
    setAge(person.age);
    setEditingId(person.id);
  };

  const handleDelete = async (id: number) => {
    await axios.delete(`http://localhost:5054/api/persons/${id}`);
    fetchPersons();
  };

  const resetForm = () => {
    setName('');
    setAge(0);
    setEditingId(null);
  };

  return (
    <div>
      <h2>Pessoas</h2>
      <input type="text" value={name} onChange={e => setName(e.target.value)} placeholder="Nome" />
      <input type="number" value={age} onChange={e => setAge(parseInt(e.target.value))} placeholder="Idade" />
      <button onClick={handleSubmit}>{editingId ? 'Editar' : 'Criar'}</button>
      <ul>
        {persons.map(p => (
          <li key={p.id}>
            {p.name} ({p.age})
            <button onClick={() => handleEdit(p)}>Editar</button>
            <button onClick={() => handleDelete(p.id)}>Deletar</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Persons;