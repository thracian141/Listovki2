<div>
    <slot />
</div>

<style>
    div {
        backdrop-filter: blur(5px);
        background-color: rgba(255, 255, 255, 0.4);
        border-radius: 1rem;
        padding:1rem;
        display: flex;
        flex-direction: column;
        align-items: center;
        width:50rem;
        margin-top:1rem;
    }
</style>