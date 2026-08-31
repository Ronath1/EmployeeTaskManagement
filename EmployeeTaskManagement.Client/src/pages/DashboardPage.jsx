import { getUser } from "../utils/authStorage";

function DashboardPage() {
  const user = getUser();

  return (
    <div className="page">
      <h1>Dashboard</h1>

      <div className="panel">
        <h2>Welcome, {user?.fullName}</h2>
        <p>Role: {user?.role}</p>
        <p>Email: {user?.email}</p>
      </div>
    </div>
  );
}

export default DashboardPage;