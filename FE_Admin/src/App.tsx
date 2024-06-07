import useRouteElement from "./hooks/useRouteElement";

function App() {
	const routeElement = useRouteElement();
	return <>{routeElement}</>;
}

export default App;
