import { useEffect, useState } from "react";
import {
  createEmployee,
  deleteEmployee,
  getEmployees,
  updateEmployee,
} from "../api/employeesApi";
import { getDepartments } from "../api/departmentsApi";
import { getUser } from "../utils/authStorage";

function EmployeesPage() {
  const user = getUser();
  const canManage = user?.role === "Admin" || user?.role === "Manager";

  const [employees, setEmployees] = useState([]);
  const [pageInfo, setPageInfo] = useState(null);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const [departments, setDepartments] = useState([]);

  // Form & Edit State
  const [editingId, setEditingId] = useState(null);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [position, setPosition] = useState("");
  const [hireDate, setHireDate] = useState("");
  const [departmentId, setDepartmentId] = useState("");
  const [submitting, setSubmitting] = useState(false);

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

  async function loadDepartments() {
    try {
      const data = await getDepartments();
      setDepartments(data);
    } catch (err) {
      setDepartments([]);
    }
  }

  useEffect(() => {
    loadEmployees();
    loadDepartments();
  }, []);

  function handleSearchSubmit(event) {
    event.preventDefault();
    loadEmployees();
  }

  function resetForm() {
    setEditingId(null);
    setFirstName("");
    setLastName("");
    setEmail("");
    setPhone("");
    setPosition("");
    setHireDate("");
    setDepartmentId("");
  }

  function handleStartEdit(employee) {
    setEditingId(employee.id);
    setFirstName(employee.firstName);
    setLastName(employee.lastName);
    setEmail(employee.email);
    setPhone(employee.phone || "");
    setPosition(employee.position);
    setHireDate(employee.hireDate ? employee.hireDate.substring(0, 10) : "");
    setDepartmentId(employee.departmentId || "");
    window.scrollTo({ top: 0, behavior: "smooth" });
  }

  async function handleSubmit(event) {
    event.preventDefault();

    try {
      setSubmitting(true);
      setError("");

      const payload = {
        firstName,
        lastName,
        email,
        phone: phone || null,
        position,
        hireDate: hireDate
          ? new Date(hireDate).toISOString()
          : new Date().toISOString(),
        departmentId: departmentId ? parseInt(departmentId, 10) : null,
      };

      if (editingId) {
        await updateEmployee(editingId, payload);
      } else {
        await createEmployee(payload);
      }

      resetForm();
      await loadEmployees();
    } catch (err) {
      const backendMessage =
        err.response?.data?.message ||
        err.response?.data ||
        "Failed to save employee.";
      setError(
        typeof backendMessage === "string"
          ? backendMessage
          : "Failed to save employee."
      );
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDelete(id, fullName) {
    const confirmed = window.confirm(
      `Are you sure you want to delete employee "${fullName}"?`
    );
    if (!confirmed) return;

    try {
      setError("");
      await deleteEmployee(id);
      if (editingId === id) {
        resetForm();
      }
      await loadEmployees();
    } catch (err) {
      setError("Failed to delete employee.");
    }
  }

  return (
    <div className="page">
      <div className="page-header">
        <div>
          <h1>Employees</h1>
          <p>View employee records and department assignments.</p>
        </div>
      </div>

      {canManage && (
        <form className="form-panel" onSubmit={handleSubmit}>
          <h3>{editingId ? "Edit Employee" : "Add New Employee"}</h3>
          <div
            className="form-row"
            style={{
              display: "grid",
              gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))",
              gap: "12px",
              marginBottom: "12px",
            }}
          >
            <div>
              <label>First Name *</label>
              <input
                type="text"
                required
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                placeholder="First name"
              />
            </div>

            <div>
              <label>Last Name *</label>
              <input
                type="text"
                required
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                placeholder="Last name"
              />
            </div>

            <div>
              <label>Email *</label>
              <input
                type="email"
                required
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="name@example.com"
              />
            </div>

            <div>
              <label>Phone</label>
              <input
                type="tel"
                value={phone}
                onChange={(e) => setPhone(e.target.value)}
                placeholder="Phone number"
              />
            </div>

            <div>
              <label>Position *</label>
              <input
                type="text"
                required
                value={position}
                onChange={(e) => setPosition(e.target.value)}
                placeholder="Job title"
              />
            </div>

            <div>
              <label>Hire Date *</label>
              <input
                type="date"
                required
                value={hireDate}
                onChange={(e) => setHireDate(e.target.value)}
              />
            </div>

            <div>
              <label>Department</label>
              <select
                value={departmentId}
                onChange={(e) => setDepartmentId(e.target.value)}
                style={{
                  width: "100%",
                  padding: "10px 12px",
                  border: "1px solid #cbd5e1",
                  borderRadius: "6px",
                  background: "white",
                }}
              >
                <option value="">No Department</option>
                {departments.map((dept) => (
                  <option key={dept.id} value={dept.id}>
                    {dept.name}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div style={{ display: "flex", gap: "10px" }}>
            <button type="submit" disabled={submitting}>
              {submitting
                ? "Saving..."
                : editingId
                ? "Save Changes"
                : "Create Employee"}
            </button>

            {editingId && (
              <button
                type="button"
                onClick={resetForm}
                style={{
                  background: "#e2e8f0",
                  color: "#334155",
                  border: "none",
                  borderRadius: "6px",
                  padding: "11px 16px",
                  fontWeight: "700",
                  cursor: "pointer",
                }}
              >
                Cancel
              </button>
            )}
          </div>
        </form>
      )}

      <form className="toolbar" onSubmit={handleSearchSubmit}>
        <input
          type="search"
          placeholder="Search employees..."
          value={search}
          onChange={(event) => setSearch(event.target.value)}
        />

        <button type="submit">Search</button>
      </form>

      {error && (
        <div className="error-message" style={{ marginBottom: "16px" }}>
          {error}
        </div>
      )}

      <div className="table-panel">
        {loading ? (
          <p style={{ padding: "16px" }}>Loading employees...</p>
        ) : employees.length === 0 ? (
          <p style={{ padding: "16px", color: "#64748b" }}>
            No employees found.
          </p>
        ) : (
          <table>
            <thead>
              <tr>
                <th>Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>Position</th>
                <th>Department</th>
                <th>Hire Date</th>
                {canManage && <th>Actions</th>}
              </tr>
            </thead>
            <tbody>
              {employees.map((employee) => (
                <tr key={employee.id}>
                  <td>
                    {employee.firstName} {employee.lastName}
                  </td>
                  <td>{employee.email}</td>
                  <td>{employee.phone || "-"}</td>
                  <td>{employee.position}</td>
                  <td>{employee.departmentName || "Not assigned"}</td>
                  <td>{new Date(employee.hireDate).toLocaleDateString()}</td>
                  {canManage && (
                    <td>
                      <div style={{ display: "flex", gap: "6px" }}>
                        <button
                          type="button"
                          onClick={() => handleStartEdit(employee)}
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#f1f5f9",
                            color: "#0f172a",
                            border: "1px solid #cbd5e1",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Edit
                        </button>
                        <button
                          type="button"
                          onClick={() =>
                            handleDelete(
                              employee.id,
                              `${employee.firstName} ${employee.lastName}`
                            )
                          }
                          style={{
                            padding: "6px 10px",
                            fontSize: "13px",
                            background: "#fee2e2",
                            color: "#991b1b",
                            border: "1px solid #fecaca",
                            borderRadius: "4px",
                            cursor: "pointer",
                          }}
                        >
                          Delete
                        </button>
                      </div>
                    </td>
                  )}
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