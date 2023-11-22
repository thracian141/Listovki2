<script lang="ts">
    import { goto } from "$app/navigation";

    let email = ''
    let password = ''

    $: allFieldsFilled = email !== '' && password !== ''

    async function handleSubmit(event: Event) {
        event.preventDefault();

        const model = {
            Email: email,
            Password: password
        };

        const response = await fetch('https://localhost:5000/authenticate/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(model),
        });

        const data = await response.json();

        if (response.ok) {
            console.log("Login successful!");
            document.cookie = `token=${data.token};path=/;Secure;SameSite=Strict;`;
            await goto('/');
            location.reload();
        } else {
            console.error("Login failed: ", data.message);
        }
    }
</script>

<form class="col-md-8" on:submit={handleSubmit}>
    <h1>Влизане в профила:</h1>
    <div class="mb-3">
        <label for="email" class="form-label">Имейл:</label>
        <input bind:value={email} type="email" class="form-control" id="email" name="email">
    </div>
    
    <div class="mb-3">
        <label for="password" class="form-label">Парола:</label>
        <input bind:value={password} type="password" class="form-control" id="password" name="password">
    </div>
    <a href="../auth/register">Нямате профил?</a><br>
    <button type="submit" class:disabled={!allFieldsFilled} class="btn btn-primary" style="display: block; margin: 0 auto;">Влизане!</button>
</form>