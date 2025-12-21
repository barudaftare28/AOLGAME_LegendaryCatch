using UnityEngine;

[SelectionBase]
public class player : MonoBehaviour
{
    [Header("Movement Attributes")]
    [SerializeField] float _moveSpeed = 5f;
    public bool canMove = true;

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;

    private Vector2 _moveDir = Vector2.zero;

    // Nama harus persis dengan image_6169b0.png
    private readonly int _animMove = Animator.StringToHash("anim_humanwalk");
    private readonly int _animIdle = Animator.StringToHash("anim_humanidle");

    private int _currentState;

    private void Update()
    {
        if (!canMove)
        {
            _rb.linearVelocity = Vector2.zero;
            _animator.CrossFade(_animIdle, 0);
            return;
        }

        player_input();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (canMove) MovementUpdate();
    }

    private void player_input() // Pastikan hanya ada SATU fungsi ini
    {
        _moveDir.x = Input.GetAxisRaw("Horizontal");
        _moveDir.y = Input.GetAxisRaw("Vertical");
    }

    private void MovementUpdate()
    {
        if (_moveDir.sqrMagnitude < 0.01f)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        _rb.linearVelocity = _moveDir.normalized * _moveSpeed;
    }

    private void UpdateAnimation()
    {
        // Handle Flip Character
        if (_moveDir.x != 0) _spriteRenderer.flipX = (_moveDir.x < 0);

        // Tentukan animasi mana yang harus dimainkan
        int stateToPlay = (_moveDir.sqrMagnitude > 0) ? _animMove : _animIdle;

        // Jika animasi yang mau dimainkan SAMA dengan yang sedang jalan, JANGAN lakukan apa-apa.
        if (_currentState == stateToPlay) return;

        // Jalankan animasi baru dan simpan state-nya
        _animator.CrossFade(stateToPlay, 0);
        _currentState = stateToPlay;
    }
}