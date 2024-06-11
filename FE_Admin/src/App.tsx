import useRouteElement from "./hooks/useRouteElement";
import { Toaster } from "@/components/ui/toaster";

function App() {
	const routeElement = useRouteElement();
	return (
		<>
			{routeElement}
			<Toaster />
		</>
	);
}

export default App;
