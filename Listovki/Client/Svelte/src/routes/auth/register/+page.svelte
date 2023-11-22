<script lang="ts">
    import {goto} from '$app/navigation';

    let email = ''
    let password = ''
    let confirmpassword = ''
    let fullname = ''
    let username = ''
    let birthdate = ''
    let egn = ''
    let privacyPolicy : boolean

    $: allFieldsFilled = email !== '' && password !== '' && confirmpassword !== '' && fullname !== '' 
        && username !== '' && birthdate !== '' && egn !== '' && privacyPolicy === true

    async function handleSubmit(event: Event) {
        event.preventDefault();

        const model = {
            FullName: fullname,
            UserName: username,
            Password: password,
            Email: email,
            BirthDate: birthdate,
            EGN: egn
        };

        const response = await fetch('https://localhost:5000/authenticate/register', {
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
    <h1>Създаване на профил:</h1>

    <div class="mb-3">
        <label for="full-name" class="form-label">Трите Имена:</label>
        <input bind:value={fullname} type="text" class="form-control" id="fullname" name="fullname">
    </div>

    <div class="mb-3">
        <label for="username" class="form-label">Потребителско име:</label>
        <input bind:value={username} type="text" class="form-control" id="username" name="username">
    </div>

    <div class="mb-3">
        <label for="email" class="form-label">Имейл:</label>
        <input bind:value={email} type="email" class="form-control" id="email" name="email">
    </div>

    <div class="mb-3">
        <label for="password" class="form-label">Парола:</label>
        <input bind:value={password} type="password" class="form-control" id="password" name="password">
    </div>

    <div class="mb-3">
        <label for="confirm-password" class="form-label">Потвърди парола:</label>
        <input bind:value={confirmpassword} type="password" class="form-control" id="confirm-password" name="confirm-password">
    </div>
    <div class="mb-3">
        <label for="birthdate" class="form-label">Рожденна дата:</label>
        <input bind:value={birthdate} type="date" class="form-control" id="birthdate" name="birthdate">
    </div>
    <div class="mb-3">
        <label for="egn" class="form-label">ЕГН:</label>
        <input bind:value={egn} type="string" class="form-control" id="egn" name="egn">
    </div>
    <div class="form-check">
        <input bind:checked={privacyPolicy} class="form-check-input" type="checkbox" id="flexCheckDefault">
        <label class="form-check-label" for="flexCheckDefault">
             Прочетох и съм съгласен с политиката за поверителност.
        </label>
    </div>
   <a href="../auth/login">Вече имате профил?</a><br>
   <button type="submit" class="btn btn-primary" class:disabled={!allFieldsFilled || confirmpassword !== password} style="display: block; margin: 0 auto;">Регистрация</button>
   
</form>