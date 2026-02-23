import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface PersonTotals {
  person: { id: number; name: string; age: number };
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

interface GeneralTotals {
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

// Componente para relatório por pessoa
const PersonReports: React.FC = () => {
  const [personTotals, setPersonTotals] = useState<PersonTotals[]>([]);
  const [general, setGeneral] = useState<GeneralTotals>({ totalIncome: 0, totalExpense: 0, balance: 0 });

  useEffect(() => {
    fetchReports();
  }, []);

  const fetchReports = async () => {
    const response = await axios.get<{ Item1: PersonTotals[], Item2: GeneralTotals }>('http://localhost:5000/api/reports/person-totals');
    setPersonTotals(response.data.Item1);
    setGeneral(response.data.Item2);
  };

  return (
    <div>
      <h2>Relatório por Pessoa</h2>
      <table>
        <thead>
          <tr>
            <th>Nome</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {personTotals.map(pt => (
            <tr key={pt.person.id}>
              <td>{pt.person.name}</td>
              <td>{pt.totalIncome}</td>
              <td>{pt.totalExpense}</td>
              <td>{pt.balance}</td>
            </tr>
          ))}
          <tr>
            <td><strong>Total Geral</strong></td>
            <td>{general.totalIncome}</td>
            <td>{general.totalExpense}</td>
            <td>{general.balance}</td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default PersonReports;