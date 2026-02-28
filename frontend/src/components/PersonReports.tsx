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
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchReports();
  }, []);

  const fetchReports = async () => {
    try {
      const response = await axios.get<{ items: PersonTotals[], general: GeneralTotals }>('http://localhost:5054/api/reports/person-totals'); // AJUSTADO: { items, general }
      console.log('Response completa do relatório por pessoa:', response.data); // Depuração
      setPersonTotals(response.data.items || []);
      setGeneral(response.data.general || { totalIncome: 0, totalExpense: 0, balance: 0 });
      setError(null);
    } catch (error: any) {
      console.error('Erro ao buscar relatório por pessoa:', error);
      setError(error.response?.data || 'Falha ao carregar relatório.');
    }
  };

  return (
    <div>
      <h2>Relatório por Pessoa</h2>
      {error ? (
        <p>{error}</p>
      ) : personTotals.length === 0 ? (
        <p>Nenhum dado disponível. Adicione transações para ver totais.</p>
      ) : (
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
                <td>{pt.totalIncome.toFixed(2)}</td>
                <td>{pt.totalExpense.toFixed(2)}</td>
                <td>{pt.balance.toFixed(2)}</td>
              </tr>
            ))}
            <tr>
              <td><strong>Total Geral</strong></td>
              <td>{general.totalIncome.toFixed(2)}</td>
              <td>{general.totalExpense.toFixed(2)}</td>
              <td>{general.balance.toFixed(2)}</td>
            </tr>
          </tbody>
        </table>
      )}
    </div>
  );
};

export default PersonReports;