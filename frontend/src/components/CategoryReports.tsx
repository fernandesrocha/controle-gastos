import React, { useState, useEffect } from 'react';
import axios from 'axios';

interface CategoryTotals {
  category: { id: number; description: string; purpose: string };
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

interface GeneralTotals {
  totalIncome: number;
  totalExpense: number;
  balance: number;
}

// Componente para relatório por categoria
const CategoryReports: React.FC = () => {
  const [categoryTotals, setCategoryTotals] = useState<CategoryTotals[]>([]);
  const [general, setGeneral] = useState<GeneralTotals>({ totalIncome: 0, totalExpense: 0, balance: 0 });
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchReports();
  }, []);

  const fetchReports = async () => {
    try {
      const response = await axios.get<{ items: CategoryTotals[], general: GeneralTotals }>('http://localhost:5054/api/reports/category-totals'); // AJUSTADO: { items, general }
      console.log('Response completa do relatório por categoria:', response.data); // Depuração
      setCategoryTotals(response.data.items || []);
      setGeneral(response.data.general || { totalIncome: 0, totalExpense: 0, balance: 0 });
      setError(null);
    } catch (error: any) {
      console.error('Erro ao buscar relatório por categoria:', error);
      setError(error.response?.data || 'Falha ao carregar relatório.');
    }
  };

  return (
    <div>
      <h2>Relatório por Categoria</h2>
      {error ? (
        <p>{error}</p>
      ) : categoryTotals.length === 0 ? (
        <p>Nenhum dado disponível. Adicione transações para ver totais.</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Descrição</th>
              <th>Receitas</th>
              <th>Despesas</th>
              <th>Saldo</th>
            </tr>
          </thead>
          <tbody>
            {categoryTotals.map(ct => (
              <tr key={ct.category.id}>
                <td>{ct.category.description}</td>
                <td>{ct.totalIncome.toFixed(2)}</td>
                <td>{ct.totalExpense.toFixed(2)}</td>
                <td>{ct.balance.toFixed(2)}</td>
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

export default CategoryReports;