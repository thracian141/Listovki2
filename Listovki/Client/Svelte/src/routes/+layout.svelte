<script>
    import Navbar from './Navbar.svelte';
    import './styles.css';
</script>


<div class="app">
    <Navbar />
    <main>
        <slot />
    </main>
</div>


<style>
	.app {
		display: flex;
		flex-direction: column;
		height: 100vh;
        width: 100vw;
        background-image: url('$lib/img/websitebackground.jpg');
        background-repeat: no-repeat;
        background-size: cover;
        background-position-y:bottom;
	}
    main {
        display: flex;
        height:100%;
        width:100%;
        flex-direction: column;
        align-items: center;
    }
</style>