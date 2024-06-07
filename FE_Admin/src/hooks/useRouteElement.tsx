import path from "@/constants/path";
import MainLayout from "@/layouts/MainLayout";
import Home from "@/pages/home";
import { Navigate, useRoutes } from "react-router-dom";
import Account from "@/pages/account";

const useRouteElement = () => {
	const routeElement = useRoutes([
		{
			path: "/",
			element: <Navigate to={path.home} replace />,
		},
		{
			index: true,
			path: path.home,
			element: (
				<MainLayout>
					<Home />
				</MainLayout>
			),
		},
		{
			path: path.account,
			element: (
				<MainLayout>
					<Account />
				</MainLayout>
			),
		},
	]);
	return routeElement;
};

export default useRouteElement;