import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import Persons from './components/Persons';
import Categories from './components/Categories';
import Transactions from './components/Transactions';
// import PersonReports from './components/PersonReports';
// import CategoryReports from './components/CategoryReports';

// App principal com rotas para cada funcionalidade.
const App: React.FC = () => {
  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li><Link to="/persons">Pessoas</Link></li>
            <li><Link to="/categories">Categorias</Link></li>
            <li><Link to="/transactions">Transações</Link></li>
{/*            <li><Link to="/person-reports">Relatório por Pessoa</Link></li>
            <li><Link to="/category-reports">Relatório por Categoria</Link></li>
*/}          </ul>
        </nav>
        <Routes>
          <Route path="/persons" element={<Persons />} />
          <Route path="/categories" element={<Categories />} />
          <Route path="/transactions" element={<Transactions />} />
{/*          <Route path="/person-reports" element={<PersonReports />} />
          <Route path="/category-reports" element={<CategoryReports />} />
*/}        </Routes>
      </div>
    </Router>
  );
};

export default App;