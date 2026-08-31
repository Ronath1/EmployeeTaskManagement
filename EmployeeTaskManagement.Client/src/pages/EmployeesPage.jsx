import { useEffect, useState } from "react";
import { getEmployees } from "../api/employeesApi";

function EmployeesPage() {
  const [employees, setEmployees] = useState([]);
  const [pageInfo, setPageInfo] = useState(null);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadEmployees() {
    try {
      setLoading(true);
      setError("");

      const data = await getEmployees({
        search,
        pageNumber: 1,
        pageSize: 10,
      });

      setEmployees(data.items);
      setPageInfo(data);
    } catch (err) {
      setError("Failed to load employees.");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadEmployees();
  }, []);

  function handleSearchSubmit(event) {
    event.preventDefault();
    loadEmployees();
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Employees</h1>
          <p>View employee records and department assignments.</p>
        </div>
      </div>

      <form className="toolbar" onSubmit={handleSearchSubmit}>
        <input
          type="search"
          placeholder="Search employees..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />

        <button type="submit">Search</button>
      </form>

      {error && <div className="error-message">{error}</div>}

      <div className="table-panel">
        {loading ? (
          <p>Loading employees...</p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Position</th>
                <th>Department</th>
                <th>Hire Date</th>
              </tr>
            </thead>
            <tbody>
              {employees.map((employee) => (
                <tr key={employee.id}>
                  <td>
                    {employee.firstName} {employee.lastName}
                  </td>
                  <td>{employee.email}</td>
                  <td>{employee.position}</td>
                  <td>{employee.departmentName || "Not assigned"}</td>
                  <td>{new Date(employee.hireDate).toLocaleDateString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {pageInfo && (
        <p className="page-meta">
          Showing {employees.length} of {pageInfo.totalCount} employees
        </p>
      )}
    </div>
  );
}

export default EmployeesPage;